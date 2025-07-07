namespace UniformesSystem.API.Models.DTOs
{
    public class EmployeeUniformReportDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;
        public int QuantityIssued { get; set; }
        public DateTime IssuanceDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}