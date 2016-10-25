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
        private readonly int _configId, _pageIndex, _pageSize;

        private readonly IConfigLogRepository _configLogRepository;

        public ConfigLogByConditionQueryer(IDisConfProcessConfig config, int configId, int pageIndex, int pageSize)
            : base(config)
        {
            this._configId = configId;
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
            return this._configLogRepository.GetByCondition(this._configId, this._pageIndex, this._pageSize);
        }
    }
}
