using System;
using System.Linq;
using System.Threading;
using ZooKeeperNet;

namespace DisConf.Web.Service.Zk
{
    public static class ZkHelper
    {
        public static bool TryGetZooKeeperConnection(string host, out ZooKeeper zk)
        {
            return TryGetZooKeeperConnection(host, out zk, new TimeSpan(0, 0, 5, 0), 3, new TimeSpan(0, 0, 1));
        }

        public static bool TryGetZooKeeperConnection(string host, out ZooKeeper zk, TimeSpan zkSessionTimeout, int maxrecheckTime, TimeSpan recheckTimeSpan)
        {
            zk = new ZooKeeper(host, zkSessionTimeout, null);

            for (int i = 0; i < maxrecheckTime; i++)
            {
                if (Equals(zk.State, ZooKeeper.States.CONNECTED))
                {
                    return true;
                }
                else
                {
                    Thread.Sleep(recheckTimeSpan);
                }
            }

            zk.Dispose();
            zk = null;
            return false;
        }



        public static void DeleteNode(ZooKeeper zk, string path)
        {
            var children = zk.GetChildren(path, false);
            if (children != null && children.Any())
            {
                foreach (var child in children)
                {
                    DeleteNode(zk, string.Format("{0}/{1}", path.TrimEnd('/'), child));
                }
            }

            zk.Delete(path, -1);
        }
    }
}
