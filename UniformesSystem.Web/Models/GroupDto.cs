using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.Web.Models
{
    public class GroupDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(10, ErrorMessage = "Group name cannot exceed 10 characters.")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int EmployeeTypeId { get; set; }
        
        public string EmployeeTypeName { get; set; } = string.Empty;
    }
}
