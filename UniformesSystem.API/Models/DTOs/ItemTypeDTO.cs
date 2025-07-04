using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class ItemTypeDTO
    {
        public int ItemTypeId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
    
    public class CreateItemTypeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
    
    public class UpdateItemTypeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
