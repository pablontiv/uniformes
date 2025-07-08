using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<PagedResultDto<EmployeeDto>> GetEmployeesAsync(int page = 1, int pageSize = 50, string? search = null)
        {
            try
            {
                var query = $"api/employees?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrWhiteSpace(search))
                    query += $"&search={Uri.EscapeDataString(search)}";

                var response = await _httpClient.GetAsync(query);
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<PagedResultDto<EmployeeDto>>();
                return result ?? new PagedResultDto<EmployeeDto>();
            }
            catch (Exception)
            {
                return new PagedResultDto<EmployeeDto>();
            }
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            try
            {
                var result = await GetEmployeesAsync(1, 1000);
                return result.Items.ToList();
            }
            catch (Exception)
            {
                return new List<EmployeeDto>();
            }
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/employees/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<EmployeeDto>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateEmployeeAsync(EmployeeDto employee)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/employees", employee);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeDto employee)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/employees/{id}", employee);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/employees/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
