using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class EmployeeTypeDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, ErrorMessage = "Employee type name cannot exceed 50 characters.")]
        public string Type { get; set; } = string.Empty;
    }
}
