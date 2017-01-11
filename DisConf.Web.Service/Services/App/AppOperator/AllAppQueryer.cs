using System.Collections.Generic;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AllAppQueryerDependent : DisConfBaseDependentProvider
    {
        public AllAppQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AllAppQueryer : DisConfQueryProcess<List<Web.Model.App>>
    {
        private readonly IAppRepository _appRepository;

        public AllAppQueryer(IDisConfProcessConfig config)
            : base(config)
        {
            this._appRepository = base.ResolveDependency<IAppRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            return true;
        }

        protected override List<Web.Model.App> Query()
        {
            return this._appRepository.GetAll();
        }
    }
}
