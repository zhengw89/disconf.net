using System.Web.Mvc;
using System.Web.Routing;

namespace DisConf.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Apps",
                url: "Apps/{pageIndex}",
                defaults: new { controller = "App", action = "AppList", pageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateApp",
                url: "CreateApp",
                defaults: new { controller = "App", action = "CreateApp" }
            );


            routes.MapRoute(
                name: "AppDetail",
                url: "Detail/{appName}/{envName}/{pageIndex}",
                defaults: new { controller = "App", action = "App", appName = UrlParameter.Optional, envName = UrlParameter.Optional, pageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateConfig",
                url: "Config/Create/{appName}/{envName}",
                defaults: new { controller = "App", action = "CreateConfig", appName = UrlParameter.Optional, envName = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "UpdateConfig",
               url: "Config/Update/{appName}/{envName}/{configName}",
               defaults: new { controller = "App", action = "UpdateConfig", appName = UrlParameter.Optional, envName = UrlParameter.Optional, configName = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Configs",
               url: "Configs/{appName}/{envName}",
               defaults: new { controller = "App", action = "GetConfigs", appName = UrlParameter.Optional, envName = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "ForceRefreshConfigs",
               url: "ForceRefreshConfigs/{appName}/{envName}",
               defaults: new { controller = "App", action = "ForceRefreshConfig", appName = UrlParameter.Optional, envName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{pageIndex}",
                defaults: new { controller = "App", action = "AppList", pageIndex = UrlParameter.Optional }
            );
        }
    }
}