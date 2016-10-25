using System.Web.Mvc;
using DisConf.Web.Helper.CustomConfig;
using DisConf.Web.Service;
using DisConf.Web.Service.Factory;

namespace DisConf.Web.Controllers.Base
{
    /// <summary>
    /// Web端受保护基类控制器（需要登陆）
    /// </summary>
    [Authorize]
    public class WebPrivateController : BaseController
    {
        /// <summary>
        /// 解析业务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected override T ResolveService<T>()
        {
            return ServiceLocator.Container.Resolve<T>(
                new ServiceConfig(HttpContext.User.Identity.Name,
                                  WebConfigHelper.DatabaseConnectionStringName,
                                  WebConfigHelper.ZookeeperHost));
        }
    }
}