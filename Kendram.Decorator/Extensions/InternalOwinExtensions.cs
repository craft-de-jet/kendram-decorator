using Microsoft.Owin;
using System;

namespace Kendram.Decorator.Extensions
{
    internal static class InternalOwinExtensions
    {
        public static string GetNgUiHelperBaseUrl(this IOwinContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            return context.Environment.GetIdentityServerBaseUrl();
        }
    }
}
