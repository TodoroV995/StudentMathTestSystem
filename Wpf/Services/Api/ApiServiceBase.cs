using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Wpf.Services
{
    public abstract class ApiServiceBase
    {
        protected readonly HttpClient _httpClient;

        protected static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        protected ApiServiceBase()
        {
            var apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                throw new InvalidOperationException("ApiBaseUrl is missing from App.config.");
            }

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            };
        }

        protected async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        }

        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data, JsonOptions);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
        }

        protected async Task PostAsync<TRequest>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data, JsonOptions);
            response.EnsureSuccessStatusCode();
        }

        protected async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}