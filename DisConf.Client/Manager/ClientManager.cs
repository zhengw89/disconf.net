using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DisConf.Client.Config;
using DisConf.Utility.Path;

namespace DisConf.Client.Manager
{
    public class ClientManager : BaseManager
    {
        /// <summary>
        /// 配置文件节点名称
        /// </summary>
        private const string DisConfConfigSectionName = "disconf";

        private static readonly object LockObj = new object();
        private static ClientManager _manager;
        public static ClientManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    lock (LockObj)
                    {
                        if (_manager == null)
                        {
                            _manager = new ClientManager();
                        }
                    }
                }

                return _manager;
            }
        }

        private DisConfConfig _config;
        /// <summary>
        /// 客户端配置信息
        /// </summary>
        public DisConfConfig Config { get { return this._config; } }

        private bool _initlized;

        /// <summary>
        /// 获取当前所有配置信息
        /// </summary>
        public Dictionary<string, string> Configs
        {
            get
            {
                if (!this._initlized)
                {
                    throw new InvalidOperationException();
                }

                return ConfigManager.Instance.Configs.ToDictionary(entry => entry.Key, entry => entry.Value);
            }
        }

        private ClientManager()
        {
            this._initlized = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            if (this._initlized) return true;

            this._config = ConfigurationManager.GetSection(DisConfConfigSectionName) as DisConfConfig;
            if (this._config == null)
            {
                Log.Error("配置信息对象初始化失败");
                return false;
            }

            ApiManager.Instance.Initialize(this._config.Host, this._config.RetryTimes, this._config.RetrySleepSeconds);

            if (!ConfigManager.Instance.Initialize(this._config.LocalPath, this._config.OnlyLocal))
            {
                Log.Error("配置管理初始化失败");
                return false;
            }

            if (!this._config.OnlyLocal)
            {
                var allConfig = ApiManager.Instance.GetByAppAndEnv(this._config.App, this._config.Env);
                if (allConfig.HasError)
                {
                    Log.Error("从 disconf web 获取所有配置失败");
                    return false;
                }
                else
                {
                    ConfigManager.Instance.Clear();
                    foreach (var key in allConfig.Data.Keys)
                    {
                        ConfigManager.Instance.CreateOrUpdate(key, allConfig.Data[key]);
                    }
                }
            }

            if (!this._config.OnlyLocal)
            {
                ZookeeperManager.Instance.NodeValueChangedHandler += Instance_NodeValueChangedHandler;
                ZookeeperManager.Instance.ZookeeperConnectedHandler += Instance_ZookeeperConnectedHandler;
                ZookeeperManager.Instance.NodeAdded += Instance_NodeAdded;
                ZookeeperManager.Instance.NodeRemove += Instance_NodeRemove;

                if (!ZookeeperManager.Instance.Initialize(this._config.ZkHost, this._config.App, this._config.Env))
                {
                    Log.Error("zookeeper初始化失败");
                    return false;
                }
            }

            this._initlized = true;
            Log.Info("Client初始化成功");

            return true;
        }

        #region Zookeeper 监控事件处理

        void Instance_NodeRemove(object sender, string nodeName)
        {
            Log.InfoFormat("获取删除配置通知 app:{0}  env:{1}  config:{2}", this._config.App, this._config.Env, nodeName);
            ConfigManager.Instance.Remove(nodeName);
        }

        void Instance_NodeAdded(object sender, string nodeName)
        {
            Log.InfoFormat("获取添加配置通知 app:{0}  env:{1}  config:{2}", this._config.App, this._config.Env, nodeName);
            var result = ApiManager.Instance.GetConfig(this._config.App, this._config.Env, nodeName);
            if (result.Error == null)
            {
                if (result.Data != null)
                {
                    var item = result.Data.SingleOrDefault();
                    ConfigManager.Instance.CreateOrUpdate(item.Key, item.Value);
                    ZookeeperManager.Instance.Watch(ZooPathManager.GetPath(this._config.App, this._config.Env, nodeName));
                }
            }
            else
            {
                Log.ErrorFormat("获取配置变更失败 app:{0}  env:{1}  config:{2}  errorMessage:{3}", this._config.App, this._config.Env, nodeName,
                    result.Error.Message);
            }
        }

        void Instance_ZookeeperConnectedHandler(object sender)
        {
            Log.Info("zookeeper连接成功，初始化watch节点");
            var configNames = ConfigManager.Instance.ConfigNames;
            foreach (var configName in configNames)
            {
                ZookeeperManager.Instance.Watch(ZooPathManager.GetPath(this._config.App, this._config.Env, configName));
            }

            ZookeeperManager.Instance.Watch(ZooPathManager.GetAddPath(this._config.App, this._config.Env));
            ZookeeperManager.Instance.Watch(ZooPathManager.GetDelPath(this._config.App, this._config.Env));
        }

        void Instance_NodeValueChangedHandler(string app, string env, string config)
        {
            Log.InfoFormat("获取配置变更通知 app:{0}  env:{1}  config:{2}", app, env, config);
            var result = ApiManager.Instance.GetConfig(app, env, config);
            if (result.Error == null)
            {
                if (result.Data != null)
                {
                    var item = result.Data.SingleOrDefault();
                    ConfigManager.Instance.CreateOrUpdate(item.Key, item.Value);
                }
            }
            else
            {
                Log.ErrorFormat("获取配置变更失败 app:{0}  env:{1}  config:{2}  errorMessage:{3}", app, env, config,
                    result.Error.Message);
            }
        }

        #endregion

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="name">配置项</param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                if (!this._initlized)
                {
                    throw new InvalidOperationException();
                }

                return ConfigManager.Instance[name];
            }
        }
    }
}