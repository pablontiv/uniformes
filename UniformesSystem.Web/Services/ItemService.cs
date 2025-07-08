using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class ItemService
    {
        private readonly HttpClient _httpClient;

        public ItemService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<ItemDto>> GetItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/items");
                response.EnsureSuccessStatusCode();
                
                var items = await response.Content.ReadFromJsonAsync<List<ItemDto>>();
                return items ?? new List<ItemDto>();
            }
            catch (Exception)
            {
                return new List<ItemDto>();
            }
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/items/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<ItemDto>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateItemAsync(ItemDto item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/items", item);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(int id, ItemDto item)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/items/{id}", item);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/items/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
