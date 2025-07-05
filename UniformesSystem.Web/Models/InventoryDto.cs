using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class InventoryDto
    {
        public int Id { get; set; }
        
        public int ItemId { get; set; }
        
        public string ItemName { get; set; } = string.Empty;
        
        public int SizeId { get; set; }
        
        public string SizeValue { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue, ErrorMessage = "Current stock cannot be negative.")]
        public int CurrentStock { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Minimum stock cannot be negative.")]
        public int MinimumStock { get; set; }
    }
}
