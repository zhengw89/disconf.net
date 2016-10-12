using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppUpdaterDependent : DisConfBaseDependentProvider
    {
        public AppUpdaterDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
        }
    }

    internal class AppUpdater : DisConfOperateProcess
    {
        private readonly int _id;
        private readonly string _name, _description;

        private readonly IAppRepository _appRepository;

        public AppUpdater(IDisConfProcessConfig config, int id, string name, string description)
            : base(config)
        {
            this._id = id;
            this._name = name;
            this._description = description;

            this._appRepository = base.ResolveDependency<IAppRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._id <= 0)
            {
                base.CacheArgumentError("Id参数错误");
                return false;
            }
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheArgumentIsNullError("App名称为空");
                return false;
            }
            if (this._appRepository.ExistsOtherByName(this._id, this._name))
            {
                base.CacheProcessError("已存在相同名称");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            var result = this._appRepository.Update(new Web.Model.App()
            {
                Description = this._description,
                Id = this._id,
                Name = this._name
            });

            if (!result)
            {
                base.CacheProcessError("App更新失败");
                return false;
            }

            return true;
        }
    }
}
