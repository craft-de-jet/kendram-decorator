using Kendram.Decorator.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace Kendram.Decorator.Services
{
    public interface IViewService
    {
        /// <summary>
        /// Loads the HTML for the login page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Stream for the HTML</returns>
        Task<Stream> Index(IndexViewModel model);
    }
}
