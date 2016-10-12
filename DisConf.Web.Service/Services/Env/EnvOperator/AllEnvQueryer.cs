using System.Collections.Generic;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Env.EnvOperator
{
    internal class AllEnvQueryerDependent : DisConfBaseDependentProvider
    {
        public AllEnvQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IEnvRepository>();
        }
    }

    internal class AllEnvQueryer : DisConfQueryProcess<List<Web.Model.Env>>
    {
        private readonly IEnvRepository _envRepository;

        public AllEnvQueryer(IDisConfProcessConfig config)
            : base(config)
        {
            this._envRepository = base.ResolveDependency<IEnvRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override List<Web.Model.Env> Query()
        {
            return this._envRepository.GetAll();
        }
    }
}
