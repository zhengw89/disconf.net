using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Env.EnvOperator
{
    internal class EnvByNameQueryerDependent : DisConfBaseDependentProvider
    {
        public EnvByNameQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IEnvRepository>();
        }
    }

    internal class EnvByNameQueryer : DisConfQueryProcess<Web.Model.Env>
    {
        private readonly string _name;

        private readonly IEnvRepository _envRepository;

        public EnvByNameQueryer(IDisConfProcessConfig config, string name)
            : base(config)
        {
            this._name = name;

            this._envRepository = base.ResolveDependency<IEnvRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheArgumentIsNullError("ENV名称为空");
                return false;
            }

            return true;
        }

        protected override Web.Model.Env Query()
        {
            return this._envRepository.GetByName(this._name);
        }
    }
}
