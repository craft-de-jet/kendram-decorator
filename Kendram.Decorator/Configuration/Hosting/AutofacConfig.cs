using Autofac;
using Autofac.Integration.WebApi;
using Kendram.Decorator.Endpoints;
using Kendram.Decorator.Services.Default;
using System;

namespace Kendram.Decorator.Configuration.Hosting
{
    internal static class AutofacConfig
    {
        public static IContainer Configure(NgUiHelperOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (options.Factory == null) throw new InvalidOperationException("null factory");

            NgUiHelperFactory fact = options.Factory;
            fact.Validate();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(options).AsSelf();

            // register view service plumbing
            if (fact.ViewService == null)
            {
                fact.ViewService = new DefaultViewServiceRegistration();
            }
            builder.Register(fact.ViewService);

            // load core controller
            builder.RegisterApiControllers(typeof(IndexController).Assembly);

            return builder.Build();
        }
        private static void Register(this ContainerBuilder builder, Registration registration, string name = null)
        {
            if (registration.Instance != null)
            {
                var reg = builder.Register(ctx => registration.Instance).SingleInstance();
                if (name != null)
                {
                    reg.Named(name, registration.DependencyType);
                }
                else
                {
                    reg.As(registration.DependencyType);
                }
                switch (registration.Mode)
                {
                    case RegistrationMode.Singleton:
                        // this is the only option when Instance is provided
                        break;
                    case RegistrationMode.InstancePerHttpRequest:
                        throw new InvalidOperationException("RegistrationMode.InstancePerHttpRequest can't be used when an Instance is provided.");
                    case RegistrationMode.InstancePerUse:
                        throw new InvalidOperationException("RegistrationMode.InstancePerUse can't be used when an Instance is provided.");
                }
            }
            else if (registration.Type != null)
            {
                var reg = builder.RegisterType(registration.Type);
                if (name != null)
                {
                    reg.Named(name, registration.DependencyType);
                }
                else
                {
                    reg.As(registration.DependencyType);
                }

                switch (registration.Mode)
                {
                    case RegistrationMode.InstancePerHttpRequest:
                        reg.InstancePerRequest(); break;
                    case RegistrationMode.Singleton:
                        reg.SingleInstance(); break;
                    case RegistrationMode.InstancePerUse:
                        // this is the default behavior
                        break;
                }
            }
            else if (registration.Factory != null)
            {
                var reg = builder.Register(ctx => registration.Factory(new AutofacDependencyResolver(ctx.Resolve<IComponentContext>())));
                if (name != null)
                {
                    reg.Named(name, registration.DependencyType);
                }
                else
                {
                    reg.As(registration.DependencyType);
                }

                switch (registration.Mode)
                {
                    case RegistrationMode.InstancePerHttpRequest:
                        reg.InstancePerRequest(); break;
                    case RegistrationMode.InstancePerUse:
                        // this is the default behavior
                        break;
                    case RegistrationMode.Singleton:
                        throw new InvalidOperationException("RegistrationMode.Singleton can't be used when using a factory function.");
                }
            }
            else
            {
                var message = "No type or factory found on registration " + registration.GetType().FullName;
                //Logger.Error(message);
                throw new InvalidOperationException(message);
            }

            foreach (var item in registration.AdditionalRegistrations)
            {
                builder.Register(item, item.Name);
            }
        }
    }
}
