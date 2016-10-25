using System;
using CommonProcess;
using ZooKeeperNet;
using System.Threading;
using DisConf.Web.Model;

namespace DisConf.Web.Service.Core.Process
{
    /// <summary>
    /// 操作核心基类
    /// </summary>
    internal abstract class DisConfBaseCoreOperateProcess : CoreOperateProcess
    {
        /// <summary>
        /// 是否需要更新Zookeeper标识
        /// </summary>
        protected readonly bool NeedUpdateZookeeper;
        /// <summary>
        /// 操作类配置信息
        /// </summary>
        protected readonly IDisConfProcessConfig Config;

        protected User User
        {
            get
            {
                if (Config == null) return null;
                return Config.User;
            }
        }

        protected DisConfBaseCoreOperateProcess(IDisConfProcessConfig config, bool needUpdateZookeeper)
            : base(config)
        {
            this.NeedUpdateZookeeper = needUpdateZookeeper;
            this.Config = config;
        }

        /// <summary>
        /// 捕获通用错误
        /// </summary>
        /// <param name="message"></param>
        protected void CacheProcessError(string message)
        {
            base.CacheError(-100, message);
        }

        /// <summary>
        /// 更新Zookeeper节点虚方法
        /// </summary>
        /// <param name="zk"></param>
        protected virtual void UpdateZookeeper(ZooKeeper zk)
        {
        }

        protected override void OnProcessStart()
        {
            //启动事务
            this.Config.Db.BeginTransaction();
            base.OnProcessStart();
        }

        protected override bool AfterRecordLog()
        {
            //更新Zookeeper
            if (this.NeedUpdateZookeeper)
            {
                using (var zk = new ZooKeeper(this.Config.ZookeeperHost, new TimeSpan(0, 0, 5, 0), null))
                {
                    //验证连接状态
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
            //完成事务
            this.Config.Db.CompleteTransaction();
            base.OnProcessSuccess();
        }

        protected override void OnProcessFail()
        {
            //回滚事务
            this.Config.Db.AbortTransaction();
            base.OnProcessFail();
        }
    }
}
