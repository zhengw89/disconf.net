using System.Collections.Generic;
using DisConf.Web.Model;

namespace DisConf.Web.Models.App
{
    public class AppDetailModel
    {
        public Model.App AppInfo { get; set; }

        public Env CurrentEnv { get; set; }

        public List<Env> AllEnv { get; set; }

        public PageList<Model.Config> Configs { get; set; }
    }
}