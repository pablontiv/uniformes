using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int GroupId { get; set; }
        
        public string? GroupName { get; set; }
        public string? EmployeeType { get; set; }
    }
    
    public class CreateEmployeeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int GroupId { get; set; }
    }
    
    public class UpdateEmployeeDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int GroupId { get; set; }
    }
}
