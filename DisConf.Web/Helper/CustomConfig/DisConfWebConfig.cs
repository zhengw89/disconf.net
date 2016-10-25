using System.Configuration;

namespace DisConf.Web.Helper.CustomConfig
{
    /// <summary>
    /// WebConfig包装
    /// </summary>
    public class DisConfWebConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("zookeepers")]
        public ZookeeperCollection Zookeepers
        {
            get { return (ZookeeperCollection)this["zookeepers"]; }
        }
    }

    public class ZookeeperCollection : ConfigurationElementCollection
    {
        public ZookeeperHostElement this[int index]
        {
            get { return BaseGet(index) as ZookeeperHostElement; }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ZookeeperHostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ZookeeperHostElement)element).Host;
        }
    }

    public class ZookeeperHostElement : ConfigurationElement
    {
        [ConfigurationProperty("host", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }
    }
}