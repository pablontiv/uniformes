using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public int ItemTypeId { get; set; }
        public string? ItemTypeName { get; set; }
        
        public int SizeId { get; set; }
        public string? SizeName { get; set; }
    }
    
    public class CreateItemDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public int ItemTypeId { get; set; }
        
        [Required]
        public int SizeId { get; set; }
    }
    
    public class UpdateItemDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public int ItemTypeId { get; set; }
        
        [Required]
        public int SizeId { get; set; }
    }
}
