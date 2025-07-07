namespace UniformesSystem.Web.Models
{
    public class LowStockReportDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public string SizeName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public int StockDeficit { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}