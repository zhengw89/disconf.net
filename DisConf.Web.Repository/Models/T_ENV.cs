using PetaPoco;

namespace DisConf.Web.Repository.Models
{
    [TableName("T_ENV")]
    [PrimaryKey("ENV_Id")]
    public class T_ENV
    {
        [Column("ENV_Id")]
        public int Id { get; set; }
        [Column("ENV_Name")]
        public string Name { get; set; }
    }
}
