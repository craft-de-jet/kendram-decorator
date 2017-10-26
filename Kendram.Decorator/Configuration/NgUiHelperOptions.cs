using System;

namespace Kendram.Decorator
{
    public class NgUiHelperOptions
    {
        public NgUiHelperFactory Factory { get; set; }
        public bool EnableIndexPage { get; set; }
        public string SiteName { get; set; }

        internal void Validate()
        {
            //throw new NotImplementedException();
        }
    }
}
