using Kendram.Decorator;
using Kendram.Decorator.Configuration.Hosting;
using System;

namespace Owin
{
    public static class UseNgUiHelperExtension
    {
        public static IAppBuilder UseNgUiHelper(this IAppBuilder app, NgUiHelperOptions options)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (options == null) throw new ArgumentNullException("options");

            options.Validate();

            var container = AutofacConfig.Configure(options);
            //app.UseAutofacMiddleware(container);

            var httpConfig = WebApiConfig.Configure(options, container);
            //app.UseAutofacWebApi(httpConfig);
            app.UseWebApi(httpConfig);

            return app;
        }
    }
}
