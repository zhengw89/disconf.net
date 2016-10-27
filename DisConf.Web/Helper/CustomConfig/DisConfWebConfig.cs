using System.Configuration;

namespace DisConf.Web.Helper.CustomConfig
{
    /// <summary>
    /// WebConfig包装
    /// </summary>
    public class DisConfWebConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("connectString", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ConnectString
        {
            get { return (string)this["connectString"]; }
            set { this["connectString"] = value; }
        }
    }
}