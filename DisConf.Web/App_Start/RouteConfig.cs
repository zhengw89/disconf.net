using System.Web.Mvc;
using System.Web.Routing;

namespace DisConf.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterAuthorizeRoutes(routes);
            RegisterAppRoutes(routes);
            RegisterConfigRoutes(routes);
            RegisterUserRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "App", action = "AppList" }
            );
        }

        private static void RegisterAuthorizeRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Authorize", action = "Login" }
            );

            routes.MapRoute(
                name: "LogOff",
                url: "LogOff",
                defaults: new { controller = "Authorize", action = "LogOff" }
            );
        }

        private static void RegisterAppRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Apps",
                url: "Apps/{appsPageIndex}",
                defaults: new { controller = "App", action = "AppList", appsPageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateApp",
                url: "CreateApp",
                defaults: new { controller = "App", action = "CreateApp" }
            );

            routes.MapRoute(
                name: "UpdateApp",
                url: "UpdateApp/{appName}",
                defaults: new { controller = "App", action = "UpdateApp", appName = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AppDetail",
                url: "Detail/{appName}/{envName}/{appDetailPageIndex}",
                defaults: new { controller = "App", action = "App", appName = UrlParameter.Optional, envName = UrlParameter.Optional, appDetailPageIndex = UrlParameter.Optional }
            );
        }

        private static void RegisterConfigRoutes(RouteCollection routes)
        {
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
               name: "ConfigLogs",
               url: "ConfigLogs/{appName}/{envName}/{configName}/{clPageIndex}",
               defaults: new { controller = "App", action = "ConfigLogs", appName = UrlParameter.Optional, envName = UrlParameter.Optional, configName = UrlParameter.Optional, clPageIndex = UrlParameter.Optional }
            );
        }

        private static void RegisterUserRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Users",
                url: "Users/{usersPageIndex}",
                defaults: new { controller = "User", action = "Users", usersPageIndex = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateUser",
                url: "CreateUser",
                defaults: new { controller = "User", action = "CreateUser" }
            );
        }
    }
}