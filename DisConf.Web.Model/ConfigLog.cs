using System;

namespace DisConf.Web.Model
{
    public class ConfigLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AppId { get; set; }
        public string AppName { get; set; }
        public int EnvId { get; set; }
        public string EnvName { get; set; }
        public int ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string PreValue { get; set; }
        public string CurValue { get; set; }
        public DateTime OptTime { get; set; }
        public DataOptType OptType { get; set; }
    }
}
