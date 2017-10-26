using Autofac;

namespace Kendram.Decorator.Services.Default
{
    internal class AutofacDependencyResolver : IDependencyResolver
    {
        readonly IComponentContext ctx;
        public AutofacDependencyResolver(IComponentContext ctx)
        {
            this.ctx = ctx;
        }

        public T Resolve<T>(string name)
        {
            if (name != null)
            {
                return ctx.ResolveNamed<T>(name);
            }

            return ctx.Resolve<T>();
        }
    }
}
