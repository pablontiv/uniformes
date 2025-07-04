using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class InventoryDTO
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? SizeName { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public bool IsLowStock => CurrentStock <= MinimumStock;
    }
    
    public class UpdateInventoryDTO
    {
        [Required]
        public int MinimumStock { get; set; }
    }
}
