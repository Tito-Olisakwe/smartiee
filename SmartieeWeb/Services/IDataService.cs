using SmartieeWeb.Models;
using System.Threading.Tasks;

namespace SmartieeWeb.Services
{
    /// <summary>
    /// Defines a contract for services that load trivia data.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Asynchronously loads trivia data.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains trivia data.</returns>
        Task<TriviaData> LoadTriviaDataAsync();
    }
}
