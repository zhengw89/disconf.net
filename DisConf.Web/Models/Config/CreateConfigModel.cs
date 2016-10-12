using System.ComponentModel.DataAnnotations;

namespace DisConf.Web.Models.Config
{
    public class CreateConfigModel
    {
        [Required]
        public int AppId { get; set; }

        public string AppName { get; set; }

        [Required]
        public int EnvId { get; set; }

        public string EnvName { get; set; }

        [Required]
        [Display(Name = "配置名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "配置值")]
        public string Value { get; set; }
    }
}