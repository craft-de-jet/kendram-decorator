using System.Threading.Tasks;

namespace Kendram.Decorator.Services.Default
{
    /// <summary>
    /// Models loading the HTML for a view.
    /// </summary>
    public interface IViewLoader
    {
        /// <summary>
        /// Loads the HTML for the named view.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<string> LoadAsync(string name);
    }
}
