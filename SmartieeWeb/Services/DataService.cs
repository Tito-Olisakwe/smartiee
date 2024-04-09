using SmartieeWeb.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartieeWeb.Services
{
    /// <summary>
    /// Provides services for loading trivia data from a specified source.
    /// Utilizes <see cref="HttpClient"/> for fetching trivia data from an external JSON file.
    /// </summary>
    public class DataService
    {
        // The HttpClient instance used for making HTTP requests to fetch trivia data
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataService"/> class with a specific <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="http">The <see cref="HttpClient"/> used for HTTP operations.</param>
        public DataService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Asynchronously loads trivia data from a predefined JSON resource.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation, with a <see cref="TriviaData"/> result containing the loaded trivia data.</returns>
        public async Task<TriviaData> LoadTriviaDataAsync()
        {
            return await _http.GetFromJsonAsync<TriviaData>("TriviaData.json");
        }
    }
}
