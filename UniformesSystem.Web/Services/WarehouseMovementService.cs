using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class WarehouseMovementService
    {
        private readonly HttpClient _httpClient;

        public WarehouseMovementService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<WarehouseMovementDto>> GetMovementsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/warehouse-movements");
                response.EnsureSuccessStatusCode();
                
                var movements = await response.Content.ReadFromJsonAsync<List<WarehouseMovementDto>>();
                return movements ?? new List<WarehouseMovementDto>();
            }
            catch (Exception)
            {
                return new List<WarehouseMovementDto>();
            }
        }

        public async Task<List<WarehouseMovementDto>> GetRecentMovementsAsync(int days = 7)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/warehouse-movements/recent?days={days}");
                response.EnsureSuccessStatusCode();
                
                var movements = await response.Content.ReadFromJsonAsync<List<WarehouseMovementDto>>();
                return movements ?? new List<WarehouseMovementDto>();
            }
            catch (Exception)
            {
                return new List<WarehouseMovementDto>();
            }
        }

        public async Task<WarehouseMovementDto?> GetMovementByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/warehouse-movements/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<WarehouseMovementDto>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateMovementAsync(WarehouseMovementDto movement)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/warehouse-movements", movement);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<WarehouseMovementDto>> GetEmployeeMovementsAsync(int employeeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/warehouse-movements/employee/{employeeId}");
                response.EnsureSuccessStatusCode();
                
                var movements = await response.Content.ReadFromJsonAsync<List<WarehouseMovementDto>>();
                return movements ?? new List<WarehouseMovementDto>();
            }
            catch (Exception)
            {
                return new List<WarehouseMovementDto>();
            }
        }
    }
}
