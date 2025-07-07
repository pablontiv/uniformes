using System.Text.Json;
using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IHttpClientFactory httpClientFactory, ILogger<ReportService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("UniformesAPI");
            _logger = logger;
        }

        public async Task<List<InventoryReportDto>> GetInventoryReportAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reports/inventory");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<InventoryReportDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<InventoryReportDto>();
                }
                
                _logger.LogError($"Failed to get inventory report. Status: {response.StatusCode}");
                return new List<InventoryReportDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching inventory report");
                return new List<InventoryReportDto>();
            }
        }

        public async Task<List<TransactionReportDto>> GetTransactionReportAsync(DateTime? startDate = null, DateTime? endDate = null, int? employeeId = null, int? itemId = null)
        {
            try
            {
                var query = new List<string>();
                
                if (startDate.HasValue)
                    query.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                
                if (endDate.HasValue)
                    query.Add($"endDate={endDate.Value:yyyy-MM-dd}");
                
                if (employeeId.HasValue)
                    query.Add($"employeeId={employeeId.Value}");
                
                if (itemId.HasValue)
                    query.Add($"itemId={itemId.Value}");

                var queryString = query.Count > 0 ? "?" + string.Join("&", query) : "";
                var response = await _httpClient.GetAsync($"api/Reports/transactions{queryString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<TransactionReportDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<TransactionReportDto>();
                }
                
                _logger.LogError($"Failed to get transaction report. Status: {response.StatusCode}");
                return new List<TransactionReportDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching transaction report");
                return new List<TransactionReportDto>();
            }
        }

        public async Task<List<EmployeeUniformReportDto>> GetEmployeeUniformReportAsync(int? employeeId = null)
        {
            try
            {
                var queryString = employeeId.HasValue ? $"?employeeId={employeeId.Value}" : "";
                var response = await _httpClient.GetAsync($"api/Reports/employee-uniforms{queryString}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<EmployeeUniformReportDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<EmployeeUniformReportDto>();
                }
                
                _logger.LogError($"Failed to get employee uniform report. Status: {response.StatusCode}");
                return new List<EmployeeUniformReportDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee uniform report");
                return new List<EmployeeUniformReportDto>();
            }
        }

        public async Task<List<LowStockReportDto>> GetLowStockReportAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reports/low-stock");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<LowStockReportDto>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new List<LowStockReportDto>();
                }
                
                _logger.LogError($"Failed to get low stock report. Status: {response.StatusCode}");
                return new List<LowStockReportDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching low stock report");
                return new List<LowStockReportDto>();
            }
        }

        public async Task<ReportSummaryDto> GetReportSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reports/summary");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ReportSummaryDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result ?? new ReportSummaryDto();
                }
                
                _logger.LogError($"Failed to get report summary. Status: {response.StatusCode}");
                return new ReportSummaryDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching report summary");
                return new ReportSummaryDto();
            }
        }
    }
}