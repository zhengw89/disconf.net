using System;
using DisConf.Utility.Path;
using DisConf.Web.Model;
using DisConf.Web.Repository.Interfaces;
using DisConf.Web.Service.Core;
using DisConf.Web.Service.Core.Process;
using PetaPoco;
using ZooKeeperNet;

namespace DisConf.Web.Service.Services.Config.ConfigOperator
{
    internal class ConfigDeleterDependent : DisConfBaseDependentProvider
    {
        public ConfigDeleterDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
            base.RegistRepository<IEnvRepository>();
            base.RegistRepository<IConfigRepository>();
            base.RegistRepository<IConfigLogRepository>();
        }
    }

    internal class ConfigDeleter : DisConfOperateProcess
    {
        private readonly int _id;

        private Web.Model.Config _config;

        private readonly IAppRepository _appRepository;
        private readonly IEnvRepository _envRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IConfigLogRepository _configLogRepository;

        public ConfigDeleter(IDisConfProcessConfig config, int id)
            : base(config, true)
        {
            this._id = id;

            this._appRepository = base.ResolveDependency<IAppRepository>();
            this._envRepository = base.ResolveDependency<IEnvRepository>();
            this._configRepository = base.ResolveDependency<IConfigRepository>();
            this._configLogRepository = base.ResolveDependency<IConfigLogRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (!this._configRepository.Exists(this._id))
            {
                base.DirectSuccessProcess();
                return true;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            this._config = this._configRepository.GetById(this._id);

            if (!this._configRepository.Delete(this._id))
            {
                base.CacheProcessError("删除配置失败");
                return false;
            }

            return true;
        }

        protected override void UpdateZookeeper(ZooKeeper zk)
        {
            var app = this._appRepository.GetById(this._config.AppId);
            var env = this._envRepository.GetById(this._config.EnvId);

            var nodePath = ZooPathManager.GetPath(app.Name, env.Name, this._config.Name);
            zk.Delete(nodePath, -1);

            var delPath = ZooPathManager.GetDelPath(app.Name, env.Name);
            if (zk.Exists(delPath, false) == null)
            {
                zk.Create(delPath, this._config.Name.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                zk.SetData(delPath, this._config.Name.GetBytes(), -1);
            }

            base.UpdateZookeeper(zk);
        }

        protected override bool RecordLogInfo()
        {
            if (!this._configLogRepository.Create(new ConfigLog()
            {
                ConfigId = this._id,
                OptTime = DateTime.Now,
                OptType = DataOptType.Delete,
                UserId = base.User.Id,
                UserName = base.User.UserName
            }))
            {
                base.CacheProcessError("记录日志失败");
                return false;
            }

            return base.RecordLogInfo();
        }
    }
}
