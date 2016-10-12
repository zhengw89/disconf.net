using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppDeleterDependent : DisConfBaseDependentProvider
    {
        public AppDeleterDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AppDeleter : DisConfOperateProcess
    {
        private readonly int _id;

        private readonly IAppRepository _appRepository;

        public AppDeleter(IDisConfProcessConfig config, int id)
            : base(config)
        {
            this._id = id;

            this._appRepository = base.ResolveDependency<IAppRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._id <= 0)
            {
                base.CacheArgumentError("Id参数错误");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            if (!this._appRepository.Exists(this._id))
            {
                base.DirectSuccessProcess();
                return true;
            }

            var result = this._appRepository.Delete(this._id);

            if (!result)
            {
                base.CacheProcessError("App删除失败");
                return false;
            }

            return true;
        }
    }
}
