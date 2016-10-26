using System.ComponentModel.DataAnnotations;

namespace DisConf.Web.Models.App
{
    public class UpdateAppModel
    {
        [Required]
        public int AppId { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string AppName { get; set; }

        [Display(Name = "描述")]
        public string AppDescription { get; set; }
    }
}