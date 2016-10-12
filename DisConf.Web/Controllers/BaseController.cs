using System.Web.Mvc;
using DisConf.Web.Helper.CustomConfig;
using DisConf.Web.Service;
using DisConf.Web.Service.Factory;

namespace DisConf.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected T ResolveService<T>()
        {
            return ServiceLocator.Container.Resolve<T>(
                new ServiceConfig(WebConfigHelper.DatabaseConnectionStringName,
                                  WebConfigHelper.ZookeeperHost));
        }
    }
}