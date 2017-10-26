using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace Kendram.Decorator.Configuration.Hosting
{
    internal static class WebApiConfig
    {
        public static HttpConfiguration Configure(NgUiHelperOptions options, ILifetimeScope container)
        {
            var config = new HttpConfiguration();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigureRoutes(options, config);

            return config;
        }

        private static void ConfigureRoutes(NgUiHelperOptions options, HttpConfiguration config)
        {
            if (options.EnableIndexPage)
            {
                config.Routes.MapHttpRoute(
                    Constants.RouteNames.Index,
                    Constants.RoutePaths.Index,
                    new { controller = "Index", action = "Get" });
            }
        }
    }
}
