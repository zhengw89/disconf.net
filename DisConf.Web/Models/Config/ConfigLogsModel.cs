using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Models.Config
{
    public class ConfigLogsModel
    {
        public List<Model.App> Apps { get; set; }
        public List<Env> Envs { get; set; }

        private string _configId;
        public string ConfigId { get { return _configId; } set { _configId = value; } }
        private string _configNameFuzzy;
        public string ConfigNameFuzzy
        {
            get
            {
                if (!string.IsNullOrEmpty(_configId)) return null;
                return _configNameFuzzy;
            }
            set { _configNameFuzzy = value; }
        }


        public Model.App App { get; set; }
        public string AppName
        {
            get
            {
                if (App == null) return null;
                return App.Name;
            }
        }
        public Env Env { get; set; }
        public string EnvName
        {
            get
            {
                if (Env == null) return null;
                return Env.Name;
            }
        }
        //public Model.Config Config { get; set; }
        //public string ConfigName
        //{
        //    get
        //    {
        //        if (Config == null) return null;
        //        return Config.Name;
        //    }
        //}

        public PageList<ConfigLog> ConfigLogs { get; set; }
    }
}