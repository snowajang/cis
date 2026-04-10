using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace cis.Models.Rest
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string rootPath = "/meetrens";
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public int statusCode { get; private set; }
        public string? rawResponse { get; private set; }

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void SetAuthorizationHeader(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        private static string BuildUrl(string rootPath, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = "/api";

            return $"{rootPath.TrimEnd('/')}/{path.TrimStart('/')}";
        }

        private async Task<HttpResponseMessage> AspFetch(
            string method,
            string path = "/api",
            object? data = null,
            string? token = null)
        {
            statusCode = 0;
            rawResponse = null;

            SetAuthorizationHeader(token);

            var url = BuildUrl(rootPath, path);
            using var request = new HttpRequestMessage(new HttpMethod(method.ToUpperInvariant()), url);

            if (data != null && request.Method != HttpMethod.Get && request.Method != HttpMethod.Delete)
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            statusCode = (int)response.StatusCode;
            return response;
        }

        private async Task<T?> ReadResponseAsync<T>(HttpResponseMessage response)
        {
            if (response.Content == null)
                return default;

            rawResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"HTTP Status: {statusCode}");
            Console.WriteLine($"Raw Response: {rawResponse}");

            if (string.IsNullOrWhiteSpace(rawResponse))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(rawResponse, _jsonOptions);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialize Error: {ex.Message}");
                return default;
            }
        }

        public async Task<T?> GetDataAsync<T>(string path = "/api", string? token = null)
        {
            var response = await AspFetch("GET", path, null, token);
            return await ReadResponseAsync<T>(response);
        }

        public async Task<T?> PostDataAsync<T>(string path = "/api", object? data = null, string? token = null)
        {
            var response = await AspFetch("POST", path, data, token);
            return await ReadResponseAsync<T>(response);
        }

        public async Task<T?> PutDataAsync<T>(string path = "/api", object? data = null, string? token = null)
        {
            var response = await AspFetch("PUT", path, data, token);
            return await ReadResponseAsync<T>(response);
        }

        public async Task<T?> DeleteDataAsync<T>(string path = "/api", string? token = null)
        {
            var response = await AspFetch("DELETE", path, null, token);
            return await ReadResponseAsync<T>(response);
        }
    }
}
