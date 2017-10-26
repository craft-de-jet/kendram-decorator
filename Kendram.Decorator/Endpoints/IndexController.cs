using Kendram.Decorator.Configuration;
using Kendram.Decorator.Configuration.Results;
using Kendram.Decorator.Services;
using Kendram.Decorator.ViewModels;
using System.Web.Http;

namespace Kendram.Decorator.Endpoints
{
    public class IndexController : ApiController
    {
        private NgUiHelperOptions options;
        private IViewService viewService;

        public IndexController(
            IViewService viewService,
            NgUiHelperOptions nuhOptions)
        {
            this.viewService = viewService;
            this.options = nuhOptions;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var indexModel = new IndexViewModel()
            {
                SiteName = options.SiteName,
            };

            return new IndexActionResult(viewService, indexModel);
        }
    }
}
