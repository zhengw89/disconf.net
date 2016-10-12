using PetaPoco;

namespace DisConf.Web.Repository.Models
{
    [TableName("T_USER")]
    [PrimaryKey("USER_ID")]
    internal class T_USER
    {
        [Column("USER_Id")]
        public int Id { get; set; }
        [Column("USER_Name")]
        public string UserName { get; set; }
        [Column("USER_Password")]
        public string Password { get; set; }
    }
}
