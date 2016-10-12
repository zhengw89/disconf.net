using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppByIdQueryerDependent : DisConfBaseDependentProvider
    {
        public AppByIdQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AppByIdQueryer : DisConfQueryProcess<Web.Model.App>
    {
        private readonly int _id;

        private readonly IAppRepository _appRepository;

        public AppByIdQueryer(IDisConfProcessConfig config, int id)
            : base(config)
        {
            this._id = id;

            this._appRepository = base.ResolveDependency<IAppRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override Web.Model.App Query()
        {
            return this._appRepository.GetById(this._id);
        }
    }
}
