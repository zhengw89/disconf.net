using System;
using PetaPoco;

namespace DisConf.Web.Repository.Models
{
    [TableName("T_CONFIG_LOG")]
    [PrimaryKey("CONFIG_LOG_Id")]
    public class T_CONFIG_LOG
    {
        [Column("CONFIG_LOG_Id")]
        public int Id { get; set; }
        [Column("USER_Id")]
        public int UserId { get; set; }
        [Column("USER_Name")]
        public string UserName { get; set; }
        [Column("CON_Id")]
        public int ConfigId { get; set; }
        [Column("OPT_Time")]
        public DateTime OptTime { get; set; }
        [Column("OPT_Type")]
        public int OptType { get; set; }
    }
}
