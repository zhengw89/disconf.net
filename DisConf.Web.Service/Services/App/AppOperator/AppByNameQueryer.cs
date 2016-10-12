using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppByNameQueryerDependent : DisConfBaseDependentProvider
    {
        public AppByNameQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AppByNameQueryer : DisConfQueryProcess<Web.Model.App>
    {
        private readonly string _name;

        private readonly IAppRepository _appRepository;

        public AppByNameQueryer(IDisConfProcessConfig config, string name)
            : base(config)
        {
            this._name = name;

            this._appRepository = base.ResolveDependency<IAppRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheProcessError("APP名称为空");
                return false;
            }

            return true;
        }

        protected override Web.Model.App Query()
        {
            return this._appRepository.GetByName(this._name);
        }
    }
}
