using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Config.ConfigLogOperator
{
    internal class ConfigLogByConditionQueryerDependent : DisConfBaseDependentProvider
    {
        public ConfigLogByConditionQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IConfigLogRepository>();
        }
    }

    internal class ConfigLogByConditionQueryer : DisConfQueryProcess<PageList<ConfigLog>>
    {
        private readonly int? _appId, _envId, _configId;
        private readonly string _configNameFuzzy;
        private readonly int _pageIndex, _pageSize;

        private readonly IConfigLogRepository _configLogRepository;

        public ConfigLogByConditionQueryer(IDisConfProcessConfig config, int? appId, int? envId, int? configId, string configNameFuzzy, int pageIndex, int pageSize)
            : base(config)
        {
            this._appId = appId;
            this._envId = envId;
            this._configId = configId;
            this._configNameFuzzy = configNameFuzzy;

            this._pageIndex = pageIndex;
            this._pageSize = pageSize;

            this._configLogRepository = base.ResolveDependency<IConfigLogRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._pageIndex <= 0 || this._pageSize <= 0)
            {
                base.CacheArgumentError("分页参数错误");
                return false;
            }

            return true;
        }

        protected override PageList<ConfigLog> Query()
        {
            if (this._configId.HasValue)
            {
                return this._configLogRepository.GetByCondition(this._appId, this._envId, this._configId,
                    this._pageIndex,this._pageSize);
            }
            else
            {
                return this._configLogRepository.GetByCondition(this._appId, this._envId, this._configNameFuzzy,
                    this._pageIndex, this._pageSize);
            }
        }
    }
}
