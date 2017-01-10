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
    internal class ConfigCreatorDependent : DisConfBaseDependentProvider
    {
        public ConfigCreatorDependent(Database db)
            : base(db)
        {
        }

        protected override void RegistDefault()
        {
            base.RegistRepository<IAppRepository>();
            base.RegistRepository<IConfigRepository>();
            base.RegistRepository<IEnvRepository>();
            base.RegistRepository<IConfigLogRepository>();
        }
    }

    internal class ConfigCreator : DisConfOperateProcess
    {
        private int _configId = -1;

        private readonly int _appId, _envId;
        private readonly string _name, _value;

        private Web.Model.App _app;
        private Web.Model.Env _env;

        private readonly IAppRepository _appRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IEnvRepository _envRepository;
        private readonly IConfigLogRepository _configLogRepository;

        public ConfigCreator(IDisConfProcessConfig config, int appId, int envId, string name, string value)
            : base(config, true)
        {
            this._appId = appId;
            this._envId = envId;
            this._name = name;
            this._value = value;

            this._appRepository = base.ResolveDependency<IAppRepository>();
            this._configRepository = base.ResolveDependency<IConfigRepository>();
            this._envRepository = base.ResolveDependency<IEnvRepository>();
            this._configLogRepository = base.ResolveDependency<IConfigLogRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (string.IsNullOrEmpty(this._name))
            {
                base.CacheArgumentError("配置项名称为空");
                return false;
            }
            if (string.IsNullOrEmpty(this._value))
            {
                base.CacheArgumentError("配置项值为空");
                return false;
            }
            if (!this._appRepository.Exists(this._appId))
            {
                base.CacheProcessError("App不存在");
                return false;
            }
            if (!this._envRepository.Exists(this._envId))
            {
                base.CacheProcessError("Env不存在");
                return false;
            }
            if (this._configRepository.ExistsByName(this._appId, this._envId, this._name))
            {
                base.CacheProcessError("已存在同名配置项");
                return false;
            }

            this._app = this._appRepository.GetById(this._appId);
            this._env = this._envRepository.GetById(this._envId);
            var nodePath = ZooPathManager.GetPath(this._app.Name, this._env.Name, this._name);
            if (nodePath.Equals(ZooPathManager.GetAddPath(this._app.Name, this._env.Name)) ||
                nodePath.Equals(ZooPathManager.GetDelPath(this._app.Name, this._env.Name)))
            {
                base.CacheProcessError("节点名称非法");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            this._configId = this._configRepository.Create(new Web.Model.Config()
            {
                AppId = this._appId,
                EnvId = this._envId,
                Name = this._name,
                Value = this._value
            });

            if (this._configId <= 0)
            {
                base.CacheProcessError("配置创建失败");
                return false;
            }

            return true;
        }

        protected override void UpdateZookeeper(ZooKeeper zk)
        {
            var nodePath = ZooPathManager.GetPath(this._app.Name, this._env.Name, this._name);

            var stat = zk.Exists(nodePath, false);
            if (stat == null)
            {
                zk.Create(nodePath, this._value.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                zk.SetData(nodePath, this._value.GetBytes(), -1);
            }

            var addPath = ZooPathManager.GetAddPath(this._app.Name, this._env.Name);
            if (zk.Exists(nodePath, false) == null)
            {
                zk.Create(addPath, this._name.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                zk.SetData(addPath, this._name.GetBytes(), -1);
            }

            base.UpdateZookeeper(zk);
        }

        protected override bool RecordLogInfo()
        {
            var app = this._appRepository.GetById(this._appId);
            var env = this._envRepository.GetById(this._envId);

            if (!this._configLogRepository.Create(new ConfigLog()
            {
                ConfigId = this._configId,
                ConfigName = this._name,
                CurValue = this._value,
                AppId = app.Id,
                AppName = app.Name,
                EnvId = env.Id,
                EnvName = env.Name,
                OptTime = DateTime.Now,
                OptType = DataOptType.Create,
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
