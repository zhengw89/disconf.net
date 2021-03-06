﻿using System.Web.Mvc;
using DisConf.Web.Helper.CustomConfig;
using DisConf.Web.Service;
using DisConf.Web.Service.Factory;

namespace DisConf.Web.Controllers.Base
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// 解析业务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T ResolveService<T>()
        {
            return ServiceLocator.Container.Resolve<T>(
                new ServiceConfig(null,
                                  WebConfigHelper.DatabaseConnectionStringName,
                                  WebConfigHelper.ZookeeperHost));
        }
    }
}