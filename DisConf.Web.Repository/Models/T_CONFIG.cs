using PetaPoco;

namespace DisConf.Web.Repository.Models
{
    [TableName("T_CONFIG")]
    [PrimaryKey("CON_Id")]
    public class T_CONFIG
    {
        [Column("CON_Id")]
        public int Id { get; set; }
        [Column("CON_Name")]
        public string Name { get; set; }
        [Column("CON_Value")]
        public string Value { get; set; }
        [Column("APP_Id")]
        public int AppId { get; set; }
        [Column("ENV_Id")]
        public int EnvId { get; set; }
    }
}
