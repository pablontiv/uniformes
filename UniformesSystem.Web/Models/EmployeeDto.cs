using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int GroupId { get; set; }
        
        public string GroupName { get; set; } = string.Empty;
        
        public string EmployeeTypeName { get; set; } = string.Empty;
        
        public int EmployeeTypeId { get; set; }
    }
}
