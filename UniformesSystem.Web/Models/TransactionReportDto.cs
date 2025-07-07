namespace UniformesSystem.Web.Models
{
    public class TransactionReportDto
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}