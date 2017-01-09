using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Models.App
{
    public class AppDetailModel
    {
        public Model.App AppInfo { get; set; }

        public Env CurrentEnv { get; set; }

        public List<Env> AllEnv { get; set; }

        public PageList<ConfigViewModel> Configs { get; set; }
    }

    public class ConfigViewModel : Model.Config
    {
        public int SyncCount { get; set; }

        public ConfigViewModel(Model.Config config = null)
        {
            if (config == null) return;

            this.Id = config.Id;
            this.Name = config.Name;
            this.Value = config.Value;
            this.AppId = config.AppId;
            this.EnvId = config.EnvId;
        }
    }
}