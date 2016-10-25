using System;

namespace DisConf.Web.Model
{
    public class ConfigLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ConfigId { get; set; }
        public DateTime OptTime { get; set; }
        public DataOptType OptType { get; set; }
    }
}
