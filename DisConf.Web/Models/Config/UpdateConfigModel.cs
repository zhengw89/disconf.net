using System.ComponentModel.DataAnnotations;

namespace DisConf.Web.Models.Config
{
    public class UpdateConfigModel
    {
        [Required]
        public int AppId { get; set; }

        [Display(Name = "APP名称")]
        public string AppName { get; set; }

        [Required]
        public int EnvId { get; set; }

        [Display(Name = "ENV名称")]
        public string EnvName { get; set; }

        [Required]
        public int ConfigId { get; set; }

        [Display(Name = "配置名称")]
        public string ConfigName { get; set; }

        [Required]
        [Display(Name = "配置值")]
        public string ConfigValue { get; set; }
    }
}