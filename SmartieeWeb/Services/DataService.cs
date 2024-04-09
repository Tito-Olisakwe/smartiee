using SmartieeWeb.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartieeWeb.Services
{
    public class DataService
    {
        private readonly HttpClient _http;

        public DataService(HttpClient http)
        {
            _http = http;
        }

        public async Task<TriviaData> LoadTriviaDataAsync()
        {
            return await _http.GetFromJsonAsync<TriviaData>("TriviaData.json");
        }
    }
}
