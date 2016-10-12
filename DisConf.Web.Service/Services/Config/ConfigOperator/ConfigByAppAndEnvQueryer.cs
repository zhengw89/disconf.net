using System.Collections.Generic;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Config.ConfigOperator
{
    internal class ConfigByAppAndEnvQueryerDependent : DisConfBaseDependentProvider
    {
        public ConfigByAppAndEnvQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IConfigRepository>();
        }
    }

    internal class ConfigByAppAndEnvQueryer : DisConfQueryProcess<List<Web.Model.Config>>
    {
        private readonly int _appId, _envId;

        private readonly IConfigRepository _configRepository;

        public ConfigByAppAndEnvQueryer(IDisConfProcessConfig config, int appId, int envId)
            : base(config)
        {
            this._appId = appId;
            this._envId = envId;

            this._configRepository = base.ResolveDependency<IConfigRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override List<Web.Model.Config> Query()
        {
            return this._configRepository.GetByAppAndEnv(this._appId, this._envId);
        }
    }
}
