﻿using System;
using System.Linq;
using System.Web.Mvc;
using DisConf.Web.Model;
using DisConf.Web.Models.App;
using DisConf.Web.Models.Config;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;

namespace DisConf.Web.Controllers
{
    public class AppController : BaseController
    {
        #region Views

        [HttpGet]
        public ActionResult AppList(int pageIndex = 1, int pageSize = 10)
        {
            var appService = base.ResolveService<IAppService>();
            var apps = appService.GetByCondition(pageIndex, pageSize);

            return View(apps);
        }

        [HttpGet]
        public ActionResult App(string appName, string envName, int pageIndex = 1, int pageSize = 10)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = app.Error
                });
            }

            var envService = base.ResolveService<IEnvService>();
            var envs = envService.GetAllEnv();
            if (envs.HasError)
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = envs.Error
                });
            }

            if (envs.Data == null || !envs.Data.Any())
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = new BizError("Environment数据为空")
                });
            }

            Env cEnv = null;
            if (!string.IsNullOrEmpty(envName))
            {
                cEnv = envs.Data.SingleOrDefault(a => a.Name.Equals(envName));
            }
            if (cEnv == null)
            {
                cEnv = envs.Data.First();
            }

            var configService = base.ResolveService<IConfigService>();
            var configs = configService.GetByCondition(app.Data.Id, cEnv.Id, pageIndex, pageSize);
            if (configs.HasError)
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = configs.Error
                });
            }

            return View(new BizResult<AppDetailModel>()
            {
                Data = new AppDetailModel()
                {
                    AllEnv = envs.Data,
                    AppInfo = app.Data,
                    Configs = configs.Data,
                    CurrentEnv = cEnv,
                }
            });
        }

        [HttpGet]
        public ActionResult CreateApp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateConfig(string appName, string envName)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return View(new BizResult<CreateConfigModel>()
                {
                    Error = app.Error
                });
            }

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvByName(envName);
            if (env.HasError)
            {
                return View(new BizResult<CreateConfigModel>()
                {
                    Error = env.Error
                });
            }

            return View(new BizResult<CreateConfigModel>()
            {
                Data = new CreateConfigModel()
                {
                    AppId = app.Data.Id,
                    AppName = appName,
                    EnvId = env.Data.Id,
                    EnvName = envName
                }
            });
        }

        [HttpGet]
        public ActionResult UpdateConfig(string appName, string envName, string configName)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return View(new BizResult<UpdateConfigModel>()
                {
                    Error = app.Error
                });
            }

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvByName(envName);
            if (env.HasError)
            {
                return View(new BizResult<UpdateConfigModel>()
                {
                    Error = env.Error
                });
            }

            var conService = base.ResolveService<IConfigService>();
            var con = conService.GetByName(app.Data.Id, env.Data.Id, configName);
            if (env.HasError)
            {
                return View(new BizResult<UpdateConfigModel>()
                {
                    Error = con.Error
                });
            }

            return View(new BizResult<UpdateConfigModel>()
            {
                Data = new UpdateConfigModel()
                {
                    AppId = app.Data.Id,
                    AppName = app.Data.Name,
                    ConfigId = con.Data.Id,
                    ConfigValue = con.Data.Value,
                    ConfigName = con.Data.Name,
                    EnvId = env.Data.Id,
                    EnvName = env.Data.Name
                }
            });
        }

        #endregion

        #region Post

        [HttpPost]
        public ActionResult CreateApp(CreateAppModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateApp", model);
            }

            var service = base.ResolveService<IAppService>();
            var result = service.CreateApp(model.Name, model.Decription);

            if (result.HasError)
            {
                ViewBag.Error = result.Error.Message;
                return View("CreateApp", model);
            }
            else
            {
                return RedirectToAction("AppList");
            }
        }

        [HttpPost]
        public ActionResult CreateConfig(BizResult<CreateConfigModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateConfig", model);
            }

            var service = base.ResolveService<IConfigService>();
            var result = service.Create(model.Data.AppId, model.Data.EnvId, model.Data.Name, model.Data.Value);

            var appService = base.ResolveService<IAppService>();
            var app = appService.GetById(model.Data.AppId);

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvById(model.Data.EnvId);

            if (result.HasError)
            {
                model.Data.AppName = app.Data.Name;
                model.Data.EnvName = env.Data.Name;

                ViewBag.Error = result.Error.Message;
                return View("CreateConfig", model);
            }
            else
            {
                return RedirectToRoute("AppDetail", new { appName = app.Data.Name, envName = env.Data.Name });
            }
        }

        [HttpPost]
        public ActionResult UpdateConfig(BizResult<UpdateConfigModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateConfig", model);
            }

            var service = base.ResolveService<IConfigService>();
            var result = service.Update(model.Data.ConfigId, model.Data.ConfigValue);

            var appService = base.ResolveService<IAppService>();
            var app = appService.GetById(model.Data.AppId);

            var envService = base.ResolveService<IEnvService>();
            var env = envService.GetEnvById(model.Data.EnvId);

            if (result.HasError)
            {
                var conService = base.ResolveService<IConfigService>();
                var con = conService.GetById(model.Data.ConfigId);

                model.Data.AppName = app.Data.Name;
                model.Data.EnvName = env.Data.Name;
                model.Data.ConfigName = con.Data.Name;

                ViewBag.Error = result.Error.Message;
                return View("UpdateConfig", model);
            }
            else
            {
                return RedirectToRoute("AppDetail", new { appName = app.Data.Name, envName = env.Data.Name });
            }
        }

        [HttpPost]
        public ActionResult DeleteConfig(int id)
        {
            var service = base.ResolveService<IConfigService>();
            var result = service.GetById(id);

            if (!result.HasError)
            {
                service.Delete(id);

                var appService = base.ResolveService<IAppService>();
                var app = appService.GetById(result.Data.AppId);

                var envService = base.ResolveService<IEnvService>();
                var env = envService.GetEnvById(result.Data.EnvId);

                return RedirectToRoute("AppDetail", new { appName = app.Data.Name, envName = env.Data.Name });
            }
            else
            {
                return RedirectToRoute("Apps");
            }
        }

        #endregion
    }
}