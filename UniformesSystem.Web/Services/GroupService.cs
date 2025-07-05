using System.Net.Http.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class GroupService
    {
        private readonly HttpClient _httpClient;

        public GroupService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
        }

        public async Task<List<GroupDto>> GetGroupsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/employees/groups");
                response.EnsureSuccessStatusCode();
                
                var groups = await response.Content.ReadFromJsonAsync<List<GroupDto>>();
                return groups ?? new List<GroupDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<GroupDto>();
            }
        }

        public async Task<GroupDto?> GetGroupByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/groups/{id}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<GroupDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return null;
            }
        }
        
        public async Task<List<EmployeeTypeDto>> GetEmployeeTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/employees/employeetypes");
                response.EnsureSuccessStatusCode();
                
                var types = await response.Content.ReadFromJsonAsync<List<EmployeeTypeDto>>();
                return types ?? new List<EmployeeTypeDto>();
            }
            catch (Exception)
            {
                // Log the exception if needed
                return new List<EmployeeTypeDto>();
            }
        }
        
        public async Task<bool> UpdateGroupAsync(GroupDto group)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/groups/{group.Id}", group);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Log the exception if needed
                return false;
            }
        }
        
        public async Task<bool> CreateGroupAsync(GroupDto group)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/groups", group);
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
