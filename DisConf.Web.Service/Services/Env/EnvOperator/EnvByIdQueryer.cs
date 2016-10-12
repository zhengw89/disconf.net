using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Env.EnvOperator
{
    internal class EnvByIdQueryerDependent : DisConfBaseDependentProvider
    {
        public EnvByIdQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IEnvRepository>();
        }
    }

    internal class EnvByIdQueryer : DisConfQueryProcess<Web.Model.Env>
    {
        private readonly int _id;

        private readonly IEnvRepository _envRepository;

        public EnvByIdQueryer(IDisConfProcessConfig config, int id)
            : base(config)
        {
            this._id = id;

            this._envRepository = base.ResolveDependency<IEnvRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override Web.Model.Env Query()
        {
            return this._envRepository.GetById(this._id);
        }
    }
}
