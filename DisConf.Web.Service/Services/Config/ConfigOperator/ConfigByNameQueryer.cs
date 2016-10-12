using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Config.ConfigOperator
{
    internal class ConfigByNameQueryerDependent : DisConfBaseDependentProvider
    {
        public ConfigByNameQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IConfigRepository>();
        }
    }

    internal class ConfigByNameQueryer : DisConfQueryProcess<Web.Model.Config>
    {
        private readonly int _appId, _envId;
        private readonly string _name;

        private readonly IConfigRepository _configRepository;

        public ConfigByNameQueryer(IDisConfProcessConfig config, int appId, int envId, string name)
            : base(config)
        {
            this._appId = appId;
            this._envId = envId;
            this._name = name;

            this._configRepository = base.ResolveDependency<IConfigRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheArgumentIsNullError("配置名称");
                return false;
            }

            return true;
        }

        protected override Web.Model.Config Query()
        {
            return this._configRepository.GetByName(this._appId, this._envId, this._name);
        }
    }
}
