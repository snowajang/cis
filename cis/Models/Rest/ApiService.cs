using System.Net.Http.Json;

namespace cis.Models.Rest
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        public int statusCode { get; set; }

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<HttpResponseMessage> AspFetch(string method, string path = "/api", Object? data=null, String? token = null)
        {
            statusCode = 0;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            var response = new HttpResponseMessage();
            if (method.ToUpper() != "GET")
            {
                response = await _httpClient.GetAsync(path);
            } else if (method.ToUpper() == "POST")
            {
                response = await _httpClient.PostAsJsonAsync(path, data);
            } else if (method.ToUpper() == "PUT")
            {
                response = await _httpClient.PutAsJsonAsync(path, data);
            } else if (method.ToUpper() == "DELETE")
            {
                response = await _httpClient.DeleteAsync(path);
            }            
            response.EnsureSuccessStatusCode();
            statusCode = (int)response.StatusCode;
            return response;
        }
        public async Task<T?> GetDataAsync<T>(string path = "/api", String? token = null)
        {   
            var response = await AspFetch("GET", path, null, token);
            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PostDataAsync<T>(string path = "/api", object? data = null, String? token = null)
        {            
            var response = await AspFetch("POST", path, null, token);
            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PutDataAsync<T>(string path = "/api", object? data = null, String? token = null)
        {
            var response = await AspFetch("PUT", path, data, token);
            response.EnsureSuccessStatusCode();

            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> DeleteDataAsync<T>(string path = "/api", String? token = null)
        {   
            var response = await AspFetch("DELETE", path, null, token);
            if (response.Content.Headers.ContentLength == 0)
                return default;

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}