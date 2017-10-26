using Kendram.Decorator.Services;

namespace Kendram.Decorator
{
    public class NgUiHelperFactory
    {
        public Registration<IViewService> ViewService { get; set; }

        internal void Validate()
        {
            //throw new NotImplementedException();
        }
    }
}
