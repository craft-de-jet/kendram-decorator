using Kendram.Decorator.Services;
using Kendram.Decorator.ViewModels;
using Microsoft.Owin;
using System;

namespace Kendram.Decorator.Configuration.Results
{
    class IndexActionResult: HtmlStreamActionResult
    {
        public IndexActionResult(IViewService viewSvc, IndexViewModel model)
            : base(async () => await viewSvc.Index(model))
        {
            if (viewSvc == null) throw new ArgumentNullException("viewSvc");
            if (model == null) throw new ArgumentNullException("model");
        }
    }
}
