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
            ApiRetryTimesPropertyName = "apiRetryTimes",
            ApiRetrySleepSecondPropertyName = "apiRetrySleepSeconds",
            ZkRetrySleepSecondsPropertyName = "zkRetrySleepSeconds",
            ZkConnectionTimeoutSecondsPropertyName = "zkConnectionTimeoutSeconds",
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
        [ConfigurationProperty(ApiRetryTimesPropertyName, IsRequired = true)]
        public int ApiRetryTimes
        {
            get { return Convert.ToInt32(base[ApiRetryTimesPropertyName]); }
            set { base[ApiRetryTimesPropertyName] = value; }
        }

        /// <summary>
        /// Config配置站点请求重试间隔秒
        /// </summary>
        [ConfigurationProperty(ApiRetrySleepSecondPropertyName, IsRequired = true)]
        public int ApiRetrySleepSeconds
        {
            get { return Convert.ToInt32(base[ApiRetrySleepSecondPropertyName]); }
            set { base[ApiRetrySleepSecondPropertyName] = value; }
        }

        /// <summary>
        /// Zookeeper连接间隔秒
        /// </summary>
        [ConfigurationProperty(ZkRetrySleepSecondsPropertyName, IsRequired = true)]
        public int ZkRetrySleepSeconds
        {
            get { return Convert.ToInt32(base[ZkRetrySleepSecondsPropertyName]); }
            set { base[ZkRetrySleepSecondsPropertyName] = value; }
        }

        /// <summary>
        /// Zookeeper连接超时
        /// </summary>
        [ConfigurationProperty(ZkConnectionTimeoutSecondsPropertyName, IsRequired = true)]
        public int ZkConnectionTimeoutSeconds
        {
            get { return Convert.ToInt32(base[ZkConnectionTimeoutSecondsPropertyName]); }
            set { base[ZkConnectionTimeoutSecondsPropertyName] = value; }
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
