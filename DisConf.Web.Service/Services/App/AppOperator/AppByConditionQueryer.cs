using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppByConditionQueryerDependent : DisConfBaseDependentProvider
    {
        public AppByConditionQueryerDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AppByConditionQueryer : DisConfQueryProcess<PageList<Web.Model.App>>
    {
        private readonly int _pageIndex, _pageSize;

        private readonly IAppRepository _appRepository;

        public AppByConditionQueryer(IDisConfProcessConfig config, int pageIndex, int pageSize)
            : base(config)
        {
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;

            this._appRepository = base.ResolveDependency<IAppRepository>();
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

        protected override PageList<Web.Model.App> Query()
        {
            return this._appRepository.GetByPage(this._pageIndex, this._pageSize);
        }
    }
}
