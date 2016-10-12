using System;
using DisConf.Utility.Path;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;
using ZooKeeperNet;

namespace DisConf.Web.Service.Services.App.AppOperator
{
    internal class AppCreatorDependent : DisConfBaseDependentProvider
    {
        public AppCreatorDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
            base.RegistRepository<IEnvRepository>();
        }
    }

    internal class AppCreator : DisConfOperateProcess
    {
        private readonly string _name, _description;

        private readonly IAppRepository _appRepository;
        private readonly IEnvRepository _envRepository;

        public AppCreator(IDisConfProcessConfig config, string name, string description)
            : base(config, true)
        {
            this._name = name;
            this._description = description;

            this._appRepository = base.ResolveDependency<IAppRepository>();
            this._envRepository = base.ResolveDependency<IEnvRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheArgumentError("App名称为空");
                return false;
            }

            if (this._appRepository.ExistsByName(this._name))
            {
                base.CacheProcessError("已存在相同名称");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            var result = this._appRepository.Create(new Web.Model.App()
            {
                Description = this._description,
                Name = this._name
            });

            if (!result)
            {
                base.CacheProcessError("App创建失败");
                return false;
            }

            return true;
        }

        protected override void UpdateZookeeper(ZooKeeper zk)
        {
            var allEnvs = this._envRepository.GetAll();
            foreach (var env in allEnvs)
            {
                var rootPath = ZooPathManager.GetRootPath(this._name, env.Name);

                var stat = zk.Exists(rootPath, false);
                if (stat == null)
                {
                    zk.Create(rootPath, DateTime.Now.Ticks.ToString().GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }
                else
                {
                    zk.SetData(rootPath, DateTime.Now.Ticks.ToString().GetBytes(), -1);
                }
            }

            base.UpdateZookeeper(zk);
        }
    }
}
