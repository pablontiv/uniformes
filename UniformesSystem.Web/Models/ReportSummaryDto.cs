namespace UniformesSystem.Web.Models
{
    public class ReportSummaryDto
    {
        public int TotalItems { get; set; }
        public int TotalInventoryQuantity { get; set; }
        public int LowStockItemsCount { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalMovements { get; set; }
        public int RecentMovements { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}