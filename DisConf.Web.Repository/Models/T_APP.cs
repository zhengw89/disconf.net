using PetaPoco;

namespace DisConf.Web.Repository.Models
{
    [TableName("T_APP")]
    [PrimaryKey("APP_Id")]
    public class T_APP
    {
        [Column("APP_Id")]
        public int Id { get; set; }
        [Column("APP_Name")]
        public string Name { get; set; }
        [Column("APP_Description")]
        public string Description { get; set; }
    }
}
