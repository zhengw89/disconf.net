﻿using System.Configuration;

namespace DisConf.Web.Helper.CustomConfig
{
    /// <summary>
    /// 管理站点WebConfig辅助类
    /// </summary>
    public static class WebConfigHelper
    {
        public const string DatabaseConnectionStringName = "DisConfDb";

        private const string DisConfConfigSectionName = "disconf";

        private static readonly object LockObj = new object();

        private static string _zookeeperHost;
        public static string ZookeeperHost
        {
            get
            {
                if (string.IsNullOrEmpty(_zookeeperHost))
                {
                    lock (LockObj)
                    {
                        if (string.IsNullOrEmpty(_zookeeperHost))
                        {
                            var disconfCon = ConfigurationManager.GetSection(DisConfConfigSectionName) as DisConfWebConfigSection;
                            if (disconfCon == null || string.IsNullOrEmpty(disconfCon.ConnectString)) return null;

                            _zookeeperHost = disconfCon.ConnectString;
                        }
                    }
                }
                return _zookeeperHost;
            }
        }
    }
}