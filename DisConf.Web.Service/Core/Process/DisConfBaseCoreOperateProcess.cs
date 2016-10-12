using System;
using CommonProcess;
using ZooKeeperNet;

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

        protected override void OnProcessSuccess()
        {
            if (this.NeedUpdateZookeeper)
            {
                using (var zk = new ZooKeeper(this.Config.ZookeeperHost, new TimeSpan(0, 0, 5, 0), null))
                {
                    UpdateZookeeper(zk);
                }
            }

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
