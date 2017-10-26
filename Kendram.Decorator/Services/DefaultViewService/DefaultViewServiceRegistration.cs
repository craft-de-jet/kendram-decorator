using Kendram.Decorator.Configuration;
using Kendram.Decorator.Extensions;
using System;

namespace Kendram.Decorator.Services.Default
{
    /// <summary>
    /// Registration for the default view service.
    /// </summary>
    public class DefaultViewServiceRegistration : DefaultViewServiceRegistration<DefaultViewService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewServiceRegistration"/> class.
        /// </summary>
        public DefaultViewServiceRegistration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewServiceRegistration"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DefaultViewServiceRegistration(DefaultViewServiceOptions options)
            : base(options)
        {
        }
    }

    /// <summary>
    /// Registration for a customer view service derived from the DefaultViewService.
    /// </summary>
    public class DefaultViewServiceRegistration<T> : Registration<IViewService, T>
        where T : DefaultViewService
    {
        const string InnerRegistrationName = "DefaultViewServiceRegistration.inner";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewServiceRegistration"/> class.
        /// </summary>
        public DefaultViewServiceRegistration()
            : this(new DefaultViewServiceOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewServiceRegistration"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public DefaultViewServiceRegistration(DefaultViewServiceOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            AdditionalRegistrations.Add(new Registration<DefaultViewServiceOptions>(options));

            if (options.ViewLoader == null)
            {
                if (options.CustomViewDirectory.IsPresent())
                {
                    options.ViewLoader = new Registration<IViewLoader>(provider =>
                    {
                        return new FileSystemWithEmbeddedFallbackViewLoader(options.CustomViewDirectory);
                        //return new FileSystemViewLoader(options.CustomViewDirectory);
                    });
                }
                else
                {
                    options.ViewLoader = new Registration<IViewLoader, FileSystemWithEmbeddedFallbackViewLoader>();
                    //options.ViewLoader = new Registration<IViewLoader, FileSystemViewLoader>();
                }
            }

            //if (options.CacheViews)
            //{
            //    AdditionalRegistrations.Add(new Registration<IViewLoader>(options.ViewLoader, InnerRegistrationName));
            //    var cache = new ResourceCache();
            //    AdditionalRegistrations.Add(new Registration<IViewLoader>(
            //        resolver => new CachingLoader(cache, resolver.Resolve<IViewLoader>(InnerRegistrationName))));
            //}
            //else
            //{
            AdditionalRegistrations.Add(options.ViewLoader);
            //}
        }
    }
}
