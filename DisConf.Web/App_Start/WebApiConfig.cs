using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DisConf.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "GetAllConfig",
            //    routeTemplate: "api/GetAllConfig",
            //    defaults: new { controller = "Api", action = "GetAllConfig" }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
