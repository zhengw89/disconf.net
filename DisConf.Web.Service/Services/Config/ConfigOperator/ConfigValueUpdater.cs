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
    internal class ConfigValueUpdaterDependent : DisConfBaseDependentProvider
    {
        public ConfigValueUpdaterDependent(Database db)
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

    internal class ConfigValueUpdater : DisConfOperateProcess
    {
        private readonly int _id;
        private readonly string _value;

        private Web.Model.Config _config;

        private readonly IAppRepository _appRepository;
        private readonly IEnvRepository _envRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IConfigLogRepository _configLogRepository;

        public ConfigValueUpdater(IDisConfProcessConfig config, int id, string value)
            : base(config, true)
        {
            this._id = id;
            this._value = value;

            this._appRepository = base.ResolveDependency<IAppRepository>();
            this._envRepository = base.ResolveDependency<IEnvRepository>();
            this._configRepository = base.ResolveDependency<IConfigRepository>();
            this._configLogRepository = base.ResolveDependency<IConfigLogRepository>();
        }

        protected override bool PreCheckProcessDataLegal()
        {
            if (this._value == null)
            {
                base.CacheProcessError("Value为空");
                return false;
            }

            return true;
        }

        protected override bool ProcessMainData()
        {
            this._config = this._configRepository.GetById(this._id);
            if (this._config == null)
            {
                base.CacheProcessError("配置信息不存在");
                return false;
            }
            this._config.Value = this._value;

            if (!this._configRepository.Update(this._config))
            {
                base.CacheProcessError("配置更新失败");
                return false;
            }

            return true;
        }

        protected override void UpdateZookeeper(ZooKeeper zk)
        {
            var app = this._appRepository.GetById(this._config.AppId);
            var env = this._envRepository.GetById(this._config.EnvId);

            var nodePath = ZooPathManager.GetPath(app.Name, env.Name, this._config.Name);
            var stat = zk.Exists(nodePath, false);
            if (stat == null)
            {
                zk.Create(nodePath, this._value.GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
            }
            else
            {
                zk.SetData(nodePath, this._value.GetBytes(), -1);
            }

            base.UpdateZookeeper(zk);
        }

        protected override bool RecordLogInfo()
        {
            if (!this._configLogRepository.Create(new ConfigLog()
            {
                ConfigId = this._id,
                OptTime = DateTime.Now,
                OptType = DataOptType.Update,
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
