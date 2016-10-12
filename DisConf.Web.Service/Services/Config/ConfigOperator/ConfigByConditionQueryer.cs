using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Config.ConfigOperator
{
    internal class ConfigByConditionQueryerDependent : DisConfBaseDependentProvider
    {
        public ConfigByConditionQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IConfigRepository>();
        }
    }

    internal class ConfigByConditionQueryer : DisConfQueryProcess<PageList<Web.Model.Config>>
    {
        private readonly int _appId, _envId, _pageIndex, _pageSize;

        private readonly IConfigRepository _configRepository;

        public ConfigByConditionQueryer(IDisConfProcessConfig config, int appId, int envId, int pageIndex, int pageSize)
            : base(config)
        {
            this._appId = appId;
            this._envId = envId;
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;

            this._configRepository = base.ResolveDependency<IConfigRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._pageIndex <= 0 || this._pageSize <= 0)
            {
                base.CacheProcessError("分页参数非法");
                return false;
            }

            return true;
        }

        protected override PageList<Web.Model.Config> Query()
        {
            return this._configRepository.GetByCondition(this._appId, this._envId, this._pageIndex, this._pageSize);
        }
    }
}
