using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class WarehouseMovementDto
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }
        
        [Required]
        public int MovementTypeId { get; set; }
        
        public string MovementTypeName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public int? EmployeeId { get; set; }
        
        public string? EmployeeName { get; set; }
        
        public List<WarehouseMovementDetailDto> Details { get; set; } = new List<WarehouseMovementDetailDto>();
    }
    
    public class WarehouseMovementDetailDto
    {
        public int Id { get; set; }
        
        public int WarehouseMovementId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        public string ItemName { get; set; } = string.Empty;
        
        [Required]
        public int SizeId { get; set; }
        
        public string SizeValue { get; set; } = string.Empty;
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
