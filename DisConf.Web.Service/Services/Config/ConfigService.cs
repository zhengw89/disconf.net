using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisConf.Utility.Path;
using DisConf.Web.Model;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Services.Config.ConfigLogOperator;
using DisConf.Web.Service.Services.Config.ConfigOperator;
using DisConf.Web.Service.Zk;
using ZooKeeperNet;

namespace DisConf.Web.Service.Services.Config
{
    internal class ConfigService : BaseService, IConfigService
    {
        public ConfigService(ServiceConfig config)
            : base(config)
        {
        }

        public BizResult<bool> Create(int appId, int envId, string name, string value)
        {
            return base.ExeProcess(db =>
            {
                var creator = new ConfigCreator(
                    base.ResloveProcessConfig<ConfigCreator>(db, true),
                    appId, envId, name, value);

                return base.ExeOperateProcess(creator);
            });
        }

        public BizResult<bool> Update(int id, string value)
        {
            return base.ExeProcess(db =>
            {
                var updater = new ConfigValueUpdater(
                    base.ResloveProcessConfig<ConfigValueUpdater>(db, true),
                    id, value);

                return base.ExeOperateProcess(updater);
            });
        }

        public BizResult<bool> Delete(int id)
        {
            return base.ExeProcess(db =>
            {
                var deleter = new ConfigDeleter(
                    base.ResloveProcessConfig<ConfigDeleter>(db, true),
                    id);

                return base.ExeOperateProcess(deleter);
            });
        }

        public BizResult<Web.Model.Config> GetById(int id)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new ConfigByIdQueryer(
                    base.ResloveProcessConfig<ConfigByIdQueryer>(db),
                    id);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<Web.Model.Config> GetByName(int appId, int envId, string name)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new ConfigByNameQueryer(
                    base.ResloveProcessConfig<ConfigByNameQueryer>(db),
                    appId, envId, name);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<List<Web.Model.Config>> GetAll(int appId, int envId)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new ConfigByAppAndEnvQueryer(
                    base.ResloveProcessConfig<ConfigByAppAndEnvQueryer>(db),
                    appId, envId);

                return base.ExeQueryProcess(queryer);
            });
        }

        public BizResult<PageList<Web.Model.Config>> GetByCondition(int appId, int envId, int pageIndex, int pageSize)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new ConfigByConditionQueryer(
                    base.ResloveProcessConfig<ConfigByConditionQueryer>(db),
                    appId, envId, pageIndex, pageSize);

                return base.ExeQueryProcess(queryer);
            });
        }

        public bool ForceRefresh(int appId, string appName, int envId, string envName)
        {
            var configs = this.GetAll(appId, envId);
            if (configs.HasError) return false;

            if (configs.Data != null && configs.Data.Any())
            {
                ZooKeeper zk = null;
                if (ZkHelper.TryGetZooKeeperConnection(this.Config.ZookeeperHost, out zk))
                {
                    using (zk)
                    {
                        foreach (var config in configs.Data)
                        {
                            var nodePath = ZooPathManager.GetPath(appName, envName, config.Name);
                            zk.SetData(nodePath, config.Value.GetBytes(), -1);
                        }
                    }
                }
            }

            return true;
        }

        public BizResult<PageList<ConfigLog>> GetConfigLogs(int configId, int pageIndex, int pageSize)
        {
            return base.ExeProcess(db =>
            {
                var queryer = new ConfigLogByConditionQueryer(
                    base.ResloveProcessConfig<ConfigLogByConditionQueryer>(db),
                    configId, pageIndex, pageSize);

                return base.ExeQueryProcess(queryer);
            });
        }

        public int GetSyncCount(ZooKeeper zk, string app, string env, string config, string value)
        {
            int count = 0;
            var nodePath = ZooPathManager.GetPath(app, env, config);
            var children = zk.GetChildren(nodePath, null);
            foreach (var child in children)
            {
                var data = zk.GetData(ZooPathManager.JoinPath(nodePath, child), null, null);
                if (Encoding.Default.GetString(data).Equals(value))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
