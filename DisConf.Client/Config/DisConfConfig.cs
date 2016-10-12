using System;
using System.Configuration;

namespace DisConf.Client.Config
{
    /// <summary>
    /// disconf客户端配置信息
    /// </summary>
    public class DisConfConfig : ConfigurationSection
    {
        private const string HostPropertyName = "host",
            RetryTimesPropertyName = "retryTimes",
            RetrySleepSecondPropertyName = "retrySleepSeconds",
            LocalPathPropertyName = "localPath",
            AppPropertyName = "app",
            EnvPropertyName = "env",
            ZkHostPropertyName = "zkHost",
            OnlyLocalPropertyName = "onlyLocal";

        /// <summary>
        /// Config配置站点Host
        /// </summary>
        [ConfigurationProperty(HostPropertyName, IsRequired = true)]
        public string Host
        {
            get { return base[HostPropertyName].ToString(); }
            set { base[HostPropertyName] = value; }
        }

        /// <summary>
        /// Config配置站点请求重试次数
        /// </summary>
        [ConfigurationProperty(RetryTimesPropertyName, IsRequired = true)]
        public int RetryTimes
        {
            get { return Convert.ToInt32(base[RetryTimesPropertyName]); }
            set { base[RetryTimesPropertyName] = value; }
        }

        /// <summary>
        /// Config配置站点请求重试间隔秒
        /// </summary>
        [ConfigurationProperty(RetrySleepSecondPropertyName, IsRequired = true)]
        public int RetrySleepSeconds
        {
            get { return Convert.ToInt32(base[RetrySleepSecondPropertyName]); }
            set { base[RetrySleepSecondPropertyName] = value; }
        }

        /// <summary>
        /// 本地配置文件路径
        /// </summary>
        [ConfigurationProperty(LocalPathPropertyName, IsRequired = true)]
        public string LocalPath
        {
            get { return base[LocalPathPropertyName].ToString(); }
            set { base[LocalPathPropertyName] = value; }
        }

        /// <summary>
        /// APP名称
        /// </summary>
        [ConfigurationProperty(AppPropertyName, IsRequired = true)]
        public string App
        {
            get { return base[AppPropertyName].ToString(); }
            set { base[AppPropertyName] = value; }
        }

        /// <summary>
        /// ENV名称
        /// </summary>
        [ConfigurationProperty(EnvPropertyName, IsRequired = true)]
        public string Env
        {
            get { return base[EnvPropertyName].ToString(); }
            set { base[EnvPropertyName] = value; }
        }

        /// <summary>
        /// Zookeeper Host
        /// </summary>
        [ConfigurationProperty(ZkHostPropertyName, IsRequired = true)]
        public string ZkHost
        {
            get { return base[ZkHostPropertyName].ToString(); }
            set { base[ZkHostPropertyName] = value; }
        }

        /// <summary>
        /// 仅使用本地配置文件
        /// </summary>
        [ConfigurationProperty(OnlyLocalPropertyName, IsRequired = true)]
        public bool OnlyLocal
        {
            get { return Convert.ToBoolean(base[OnlyLocalPropertyName].ToString()); }
            set { base[OnlyLocalPropertyName] = value; }
        }
    }
}
