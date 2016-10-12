using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DisConf.Client.Manager
{
    /// <summary>
    /// 配置信息
    /// </summary>
    internal class ConfigManager : BaseManager
    {
        private static readonly object LockObj = new object();
        private static ConfigManager _manager;
        public static ConfigManager Instance
        {
            get
            {
                if (_manager == null)
                {
                    lock (LockObj)
                    {
                        if (_manager == null)
                        {
                            _manager = new ConfigManager();
                        }
                    }
                }

                return _manager;
            }
        }

        /// <summary>
        /// 本地配置文件路径
        /// </summary>
        private string _localPath;

        /// <summary>
        /// 配置信息缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, string> _configDic;
        public ConcurrentDictionary<string, string> Configs
        {
            get
            {
                if (!this._initlized)
                {
                    throw new InvalidOperationException();
                }

                return this._configDic;
            }
        }

        private bool _initlized;

        public string this[string name]
        {
            get
            {
                if (!this._initlized)
                {
                    throw new InvalidOperationException();
                }

                if (this._configDic.ContainsKey(name)) return this._configDic[name];
                return null;
            }
        }
        public List<string> ConfigNames
        {
            get
            {
                if (!this._initlized)
                {
                    throw new InvalidOperationException();
                }

                return this._configDic.Keys.ToList();
            }
        }

        private ConfigManager()
        {
            this._configDic = new ConcurrentDictionary<string, string>();

            this._initlized = false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="localPath">本地配置文件路径</param>
        /// <param name="loadLocal">预加载本地配置</param>
        /// <returns></returns>
        public bool Initialize(string localPath, bool loadLocal)
        {
            if (_initlized) return true;

            this._localPath = localPath;

            if (loadLocal)
            {
                var localConfigDic = this.LoadConfigFromFile(this._localPath);
                if (localConfigDic == null)
                {
                    Log.ErrorFormat("本地Config文件错误 {0}", this._localPath);
                    return false;
                }

                foreach (var key in localConfigDic.Keys)
                {
                    if (!this._configDic.TryAdd(key, localConfigDic[key]))
                    {
                        Log.ErrorFormat("加载缓存配置失败 key:{0}  value:{1}", key, localConfigDic[key]);
                        return false;
                    }
                }
            }

            this._initlized = true;

            return true;
        }

        /// <summary>
        /// 创建或更新配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="value">配置值</param>
        public void CreateOrUpdate(string name, string value)
        {
            if (!this._initlized)
            {
                throw new InvalidOperationException();
            }

            var configDic = this.LoadConfigFromFile(this._localPath);
            if (configDic.ContainsKey(name))
            {
                configDic[name] = value;
            }
            else
            {
                configDic.Add(name, value);
            }
            this.WirteConfigToFile(this._localPath, configDic);

            if (this._configDic.ContainsKey(name))
            {
                this._configDic[name] = value;
            }
            else
            {
                this._configDic.TryAdd(name, value);
            }
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        public void Remove(string name)
        {
            if (!this._initlized)
            {
                throw new InvalidOperationException();
            }

            var configDic = this.LoadConfigFromFile(this._localPath);
            if (configDic.ContainsKey(name))
            {
                configDic.Remove(name);
                this.WirteConfigToFile(this._localPath, configDic);
            }

            if (this._configDic.ContainsKey(name))
            {
                string value;
                this._configDic.TryRemove(name, out value);
            }
        }

        /// <summary>
        /// 清理本地配置信息
        /// </summary>
        public void Clear()
        {
            if (!this._initlized)
            {
                throw new InvalidOperationException();
            }

            this._configDic.Clear();
            this.WirteConfigToFile(this._localPath, new Dictionary<string, string>());
        }

        private bool WirteConfigToFile(string fileName, Dictionary<string, string> config)
        {
            lock (LockObj)
            {
                Log.Info("更新本地配置文件");
                var content = JsonConvert.SerializeObject(config);

                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.SetLength(0);

                        sw.Write(content);
                        return true;
                    }
                }
            }
        }

        private Dictionary<string, string> LoadConfigFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            lock (LockObj)
            {
                Log.InfoFormat("加载本地配置文件 path:{0}", fileName);
                var sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(sb.ToString());
            }
        }

    }
}