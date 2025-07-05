using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int ItemTypeId { get; set; }
        
        public string ItemTypeName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public int SizeId { get; set; }
        
        public string SizeValue { get; set; } = string.Empty;
        
        public SizeSystem SizeSystem { get; set; }
        
        // Display helper
        public string FormattedSize => $"{SizeValue} ({SizeSystem})";
    }
}
