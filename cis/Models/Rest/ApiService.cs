using System.Net.Http.Json;

namespace cis.Models.Rest
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetDataAsync<T>(string urlstring = "/api")
        {
            var response = await _httpClient.GetAsync(urlstring);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PostDataAsync<T>(string urlstring = "/api", object? data = null)
        {
            var response = await _httpClient.PostAsJsonAsync(urlstring, data);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PutDataAsync<T>(string urlstring = "/api", object? data = null)
        {
            var response = await _httpClient.PutAsJsonAsync(urlstring, data);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> DeleteDataAsync<T>(string urlstring = "/api")
        {
            var response = await _httpClient.DeleteAsync(urlstring);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}