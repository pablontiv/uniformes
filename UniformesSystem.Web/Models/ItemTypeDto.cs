using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class ItemTypeDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "Item type name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public bool IsUniversal { get; set; }
        
        public List<int> AssignedEmployeeTypeIds { get; set; } = new List<int>();
    }
}
