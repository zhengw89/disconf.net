using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DisConf.Web.Controllers.Base;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Controllers
{
    /// <summary>
    /// 接口控制器
    /// </summary>
    public class ApiController : BaseController
    {
        [HttpGet]
        public ActionResult GetAllConfig(string appName, string envName)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = app.Error
                }, JsonRequestBehavior.AllowGet);
            }

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvByName(envName);
            if (env.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = env.Error
                }, JsonRequestBehavior.AllowGet);
            }

            var conService = base.ResolveService<IConfigService>();
            var cons = conService.GetAll(app.Data.Id, env.Data.Id);
            if (cons.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = cons.Error
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new BizResult<Dictionary<string, string>>()
            {
                Data = cons.Data.ToDictionary(config => config.Name, config => config.Value)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetConfigValue(string appName, string envName, string configName)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = app.Error
                }, JsonRequestBehavior.AllowGet);
            }

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvByName(envName);
            if (env.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = env.Error
                }, JsonRequestBehavior.AllowGet);
            }

            var conService = base.ResolveService<IConfigService>();
            var con = conService.GetByName(app.Data.Id, env.Data.Id, configName);
            if (con.HasError)
            {
                return Json(new BizResult<Dictionary<string, string>>()
                {
                    Error = con.Error
                }, JsonRequestBehavior.AllowGet);
            }

            var data = new Dictionary<string, string>();
            if (con.Data != null)
            {
                data.Add(con.Data.Name, con.Data.Value);
            }

            return Json(new BizResult<Dictionary<string, string>>()
            {
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}