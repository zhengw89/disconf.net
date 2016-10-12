using System.Configuration;

namespace DisConf.Web.Helper.CustomConfig
{
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
                            if (disconfCon == null || disconfCon.Zookeepers == null || disconfCon.Zookeepers.Count == 0) return null;

                            _zookeeperHost = disconfCon.Zookeepers[0].Host;


                        }
                    }
                }
                return _zookeeperHost;
            }
        }
    }
}