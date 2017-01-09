using System;
using System.Threading;
using ZooKeeperNet;

namespace DisConf.Web.Service.Zk
{
    public static class ZkHelper
    {
        public static bool TryGetZooKeeperConnection(string host, out ZooKeeper zk)
        {
            zk = new ZooKeeper(host, new TimeSpan(0, 0, 5, 0), null);

            int retry = 3;
            for (int i = 0; i < retry; i++)
            {
                if (Equals(zk.State, ZooKeeper.States.CONNECTED))
                {
                    break;
                }
                else
                {
                    Thread.Sleep(new TimeSpan(0, 0, 1));
                }
            }
            if (!Equals(zk.State, ZooKeeper.States.CONNECTED))
            {
                zk = null;
                return false;
            }

            return true;
        }
    }
}
