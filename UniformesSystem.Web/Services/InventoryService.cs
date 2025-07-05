using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class InventoryService
    {
        private readonly HttpClient _httpClient;

        public InventoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<InventoryDto>> GetInventoryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/inventory");
                response.EnsureSuccessStatusCode();
                
                var inventory = await response.Content.ReadFromJsonAsync<List<InventoryDto>>();
                return inventory ?? new List<InventoryDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<InventoryDto>();
            }
        }

        public async Task<List<InventoryDto>> GetLowStockItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/inventory/low-stock");
                response.EnsureSuccessStatusCode();
                
                var inventory = await response.Content.ReadFromJsonAsync<List<InventoryDto>>();
                return inventory ?? new List<InventoryDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<InventoryDto>();
            }
        }

        public async Task<InventoryDto?> GetInventoryByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/inventory/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<InventoryDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return null;
            }
        }

        public async Task<bool> UpdateInventoryStockLevelsAsync(int id, InventoryDto inventory)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/inventory/{id}/stock-levels", inventory);
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
