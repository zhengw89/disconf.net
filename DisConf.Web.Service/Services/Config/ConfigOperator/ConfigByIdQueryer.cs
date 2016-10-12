using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.Config.ConfigOperator
{
    internal class ConfigByIdQueryerDependent : DisConfBaseDependentProvider
    {
        public ConfigByIdQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IConfigRepository>();
        }
    }

    internal class ConfigByIdQueryer : DisConfQueryProcess<Web.Model.Config>
    {
        private readonly int _id;

        private readonly IConfigRepository _configRepository;

        public ConfigByIdQueryer(IDisConfProcessConfig config, int id)
            : base(config)
        {
            this._id = id;

            this._configRepository = base.ResolveDependency<IConfigRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override Web.Model.Config Query()
        {
            return this._configRepository.GetById(this._id);
        }
    }
}
