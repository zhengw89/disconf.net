using System.Collections.Generic;
using DisConf.Web.Model;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Services.Config.ConfigOperator;

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
                    base.ResloveProcessConfig<ConfigCreator>(db),
                    appId, envId, name, value);

                return base.ExeOperateProcess(creator);
            });
        }

        public BizResult<bool> Update(int id, string value)
        {
            return base.ExeProcess(db =>
            {
                var updater = new ConfigValueUpdater(
                    base.ResloveProcessConfig<ConfigValueUpdater>(db),
                    id, value);

                return base.ExeOperateProcess(updater);
            });
        }

        public BizResult<bool> Delete(int id)
        {
            return base.ExeProcess(db =>
            {
                var deleter = new ConfigDeleter(
                    base.ResloveProcessConfig<ConfigDeleter>(db),
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
    }
}
