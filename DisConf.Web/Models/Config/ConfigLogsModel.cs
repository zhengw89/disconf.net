using DisConf.Web.Model;

namespace DisConf.Web.Models.Config
{
    public class ConfigLogsModel
    {
        public string AppName { get; set; }

        public string EnvName { get; set; }

        public string ConfigName { get; set; }

        public PageList<ConfigLog> ConfigLogs { get; set; }
    }
}