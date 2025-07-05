using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class SizeService
    {
        private readonly HttpClient _httpClient;

        public SizeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<SizeDto>> GetSizesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/items/sizes");
                response.EnsureSuccessStatusCode();
                
                var sizes = await response.Content.ReadFromJsonAsync<List<SizeDto>>();
                return sizes ?? new List<SizeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<SizeDto>();
            }
        }

        public async Task<SizeDto?> GetSizeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/items/sizes/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<SizeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return null;
            }
        }

        public async Task<bool> CreateSizeAsync(SizeDto size)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/items/sizes", size);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> UpdateSizeAsync(int id, SizeDto size)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/items/sizes/{id}", size);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> DeleteSizeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/items/sizes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }
    }
}
