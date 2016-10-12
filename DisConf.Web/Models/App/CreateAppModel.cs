using System.ComponentModel.DataAnnotations;

namespace DisConf.Web.Models.App
{
    public class CreateAppModel
    {
        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "描述")]
        public string Decription { get; set; }
    }
}