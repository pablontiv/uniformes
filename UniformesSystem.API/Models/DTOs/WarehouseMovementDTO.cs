using System.ComponentModel.DataAnnotations;
using UniformesSystem.Database.Models;

namespace UniformesSystem.API.Models.DTOs
{
    public class WarehouseMovementDTO
    {
        public int WarehouseMovementId { get; set; }
        public DateTime Date { get; set; }
        public MovementType MovementType { get; set; }
        public string? Notes { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public List<WarehouseMovementDetailDTO> Details { get; set; } = new();
    }
    
    public class WarehouseMovementDetailDTO
    {
        public int WarehouseMovementDetailId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? SizeName { get; set; }
        public int Quantity { get; set; }
    }
    
    public class CreateWarehouseMovementDTO
    {
        [Required]
        public MovementType MovementType { get; set; }
        
        public string? Notes { get; set; }
        
        // Only required for issuance movements
        public int? EmployeeId { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "At least one detail item is required")]
        public List<CreateWarehouseMovementDetailDTO> Details { get; set; } = new();
    }
    
    public class CreateWarehouseMovementDetailDTO
    {
        [Required]
        public int ItemId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
