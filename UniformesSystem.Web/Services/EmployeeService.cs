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

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/employees");
                response.EnsureSuccessStatusCode();
                
                var employees = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>();
                return employees ?? new List<EmployeeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
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
                // Log the exception if needed
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
                // Log the exception if needed
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
                // Log the exception if needed
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
                // Log the exception if needed
                return false;
            }
        }
    }
}
