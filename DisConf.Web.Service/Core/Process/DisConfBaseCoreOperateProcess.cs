using System;
using CommonProcess;
using ZooKeeperNet;
using System.Threading;

namespace DisConf.Web.Service.Core.Process
{
    internal abstract class DisConfBaseCoreOperateProcess : CoreOperateProcess
    {
        protected readonly bool NeedUpdateZookeeper;
        protected readonly IDisConfProcessConfig Config;

        protected DisConfBaseCoreOperateProcess(IDisConfProcessConfig config, bool needUpdateZookeeper)
            : base(config)
        {
            this.NeedUpdateZookeeper = needUpdateZookeeper;
            this.Config = config;
        }

        protected void CacheProcessError(string message)
        {
            base.CacheError(-100, message);
        }

        protected virtual void UpdateZookeeper(ZooKeeper zk)
        {
        }

        protected override void OnProcessStart()
        {
            this.Config.Db.BeginTransaction();
            base.OnProcessStart();
        }

        protected override bool AfterRecordLog()
        {
            if (this.NeedUpdateZookeeper)
            {
                using (var zk = new ZooKeeper(this.Config.ZookeeperHost, new TimeSpan(0, 0, 5, 0), null))
                {
                    for (int i = 0; i < 3; i++)
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
                    if (Equals(zk.State, ZooKeeper.States.CONNECTED))
                    {
                        UpdateZookeeper(zk);
                    }
                    else
                    {
                        this.CacheProcessError("Zookeeper更新失败");
                        return false;
                    }
                }
            }

            return base.AfterRecordLog();
        }

        protected override void OnProcessSuccess()
        {
            this.Config.Db.CompleteTransaction();
            base.OnProcessSuccess();
        }

        protected override void OnProcessFail()
        {
            this.Config.Db.AbortTransaction();
            base.OnProcessFail();
        }
    }
}
