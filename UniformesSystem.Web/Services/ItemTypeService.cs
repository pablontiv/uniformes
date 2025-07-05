using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class ItemTypeService
    {
        private readonly HttpClient _httpClient;

        public ItemTypeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<ItemTypeDto>> GetItemTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/items/types");
                response.EnsureSuccessStatusCode();
                
                var itemTypes = await response.Content.ReadFromJsonAsync<List<ItemTypeDto>>();
                return itemTypes ?? new List<ItemTypeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<ItemTypeDto>();
            }
        }

        public async Task<ItemTypeDto?> GetItemTypeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/items/types/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<ItemTypeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return null;
            }
        }

        public async Task<bool> CreateItemTypeAsync(ItemTypeDto itemType)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/items/types", itemType);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> UpdateItemTypeAsync(int id, ItemTypeDto itemType)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/items/types/{id}", itemType);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }

        public async Task<bool> DeleteItemTypeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/items/types/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }
        
        public async Task<List<EmployeeTypeDto>> GetAssignedEmployeeTypesAsync(int itemTypeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/items/types/{itemTypeId}/employeetypes");
                response.EnsureSuccessStatusCode();
                
                var employeeTypes = await response.Content.ReadFromJsonAsync<List<EmployeeTypeDto>>();
                return employeeTypes ?? new List<EmployeeTypeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<EmployeeTypeDto>();
            }
        }
        
        public async Task<bool> AssignToEmployeeTypeAsync(int itemTypeId, int employeeTypeId)
        {
            try
            {
                var assignment = new { ItemTypeId = itemTypeId, EmployeeTypeId = employeeTypeId };
                var response = await _httpClient.PostAsJsonAsync($"api/items/types/{itemTypeId}/employeetypes", assignment);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }
        
        public async Task<bool> RemoveFromEmployeeTypeAsync(int itemTypeId, int employeeTypeId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/items/types/{itemTypeId}/employeetypes/{employeeTypeId}");
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
