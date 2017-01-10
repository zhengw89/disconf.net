using System;
using System.Linq;
using System.Web.Mvc;
using DisConf.Web.Controllers.Base;
using DisConf.Web.Helper.CustomConfig;
using DisConf.Web.Helper.CustomResult;
using DisConf.Web.Model;
using DisConf.Web.Models.App;
using DisConf.Web.Models.Config;
using DisConf.Web.Service.Interfaces;
using DisConf.Web.Service.Model;
using DisConf.Web.Service.Zk;
using ZooKeeperNet;

namespace DisConf.Web.Controllers
{
    /// <summary>
    /// 应用程序相关控制器
    /// </summary>
    public class AppController : WebPrivateController
    {
        #region Views

        [HttpGet]
        public ActionResult AppList([Bind(Prefix = "appsPageIndex")]int pageIndex = 1, int pageSize = 10)
        {
            var appService = base.ResolveService<IAppService>();
            var apps = appService.GetByCondition(pageIndex, pageSize);

            return View(apps);
        }

        [HttpGet]
        public ActionResult App(string appName, string envName, [Bind(Prefix = "appDetailPageIndex")] int pageIndex = 1, int pageSize = 10)
        {
            #region App

            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = app.Error
                });
            }

            #endregion

            #region Env

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

            #endregion

            #region Conf

            var configService = base.ResolveService<IConfigService>();
            var configs = configService.GetByCondition(app.Data.Id, cEnv.Id, pageIndex, pageSize);
            if (configs.HasError)
            {
                return View(new BizResult<AppDetailModel>()
                {
                    Error = configs.Error
                });
            }

            var configData = new PageList<ConfigViewModel>();
            configData.CopyPageInfo(configs.Data);
            ZooKeeper zk = null;
            if (ZkHelper.TryGetZooKeeperConnection(WebConfigHelper.ZookeeperHost, out zk))
            {
                using (zk)
                {
                    foreach (var config in configs.Data)
                    {
                        var con = new ConfigViewModel(config)
                        {
                            SyncCount = configService.GetSyncCount(zk, appName, cEnv.Name, config.Name, config.Value)
                        };

                        configData.Add(con);
                    }
                }
            }

            #endregion

            return View(new BizResult<AppDetailModel>()
            {
                Data = new AppDetailModel()
                {
                    AllEnv = envs.Data,
                    AppInfo = app.Data,
                    //Configs = configs.Data,
                    Configs = configData,
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
        public ActionResult UpdateApp(string appName)
        {
            var appService = base.ResolveService<IAppService>();
            var app = appService.GetByName(appName);
            if (app.HasError)
            {
                return View(new BizResult<UpdateAppModel>() { Error = app.Error });
            }

            return View(new BizResult<UpdateAppModel>()
            {
                Data = new UpdateAppModel()
                {
                    AppDescription = app.Data.Description,
                    AppId = app.Data.Id,
                    AppName = app.Data.Name
                }
            });
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

        [HttpGet]
        public ActionResult ConfigLogs(string appName, string envName, string configName, [Bind(Prefix = "clPageIndex")]int pageIndex = 1, int pageSize = 10)
        {
            var app = base.ResolveService<IAppService>().GetByName(appName).Data;
            var env = base.ResolveService<IEnvService>().GetEnvByName(envName).Data;
            var configService = base.ResolveService<IConfigService>();
            var config = configService.GetByName(app.Id, env.Id, configName).Data;

            return View(new ConfigLogsModel()
            {
                AppName = app.Name,
                EnvName = env.Name,
                ConfigName = config.Name,
                ConfigLogs = configService.GetConfigLogs(config.Id, pageIndex, pageSize).Data
            });
        }

        #endregion

        #region Get

        [HttpGet]
        public ActionResult GetConfigs(string appName, string envName)
        {
            var app = base.ResolveService<IAppService>().GetByName(appName);
            if (app.HasError)
            {
                return new Http404Result();
            }

            var env = base.ResolveService<IEnvService>().GetEnvByName(envName);
            if (env.HasError)
            {
                return new Http404Result();
            }

            var configs = base.ResolveService<IConfigService>().GetAll(app.Data.Id, env.Data.Id);
            if (configs.HasError)
            {
                return new Http404Result();
            }

            return Json(configs.Data.ToDictionary(a => a.Name, a => a.Value), JsonRequestBehavior.AllowGet);
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
        public ActionResult UpdateApp(BizResult<UpdateAppModel> model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateApp", model);
            }

            var service = base.ResolveService<IAppService>();
            var result = service.Update(model.Data.AppId, model.Data.AppName, model.Data.AppDescription);

            if (result.HasError)
            {
                model.Error = result.Error;
                return View("UpdateApp", model);
            }

            return RedirectToRoute("Apps");
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

        [HttpPost]
        public ActionResult ForceRefreshConfig(string appName, string envName)
        {
            var app = base.ResolveService<IAppService>().GetByName(appName);
            if (app.HasError)
            {
                throw new NotImplementedException();
            }

            var env = base.ResolveService<IEnvService>().GetEnvByName(envName);
            if (env.HasError)
            {
                throw new NotImplementedException();
            }

            ZooKeeper zk = null;
            if (ZkHelper.TryGetZooKeeperConnection(WebConfigHelper.ZookeeperHost, out zk))
            {
                using (zk)
                {
                    if (!base.ResolveService<IConfigService>().ForceRefresh(zk, app.Data.Id, appName, env.Data.Id, envName))
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return RedirectToRoute("AppDetail", new { appName = appName, envName = envName, pageIndex = 1 });
        }

        #endregion
    }
}