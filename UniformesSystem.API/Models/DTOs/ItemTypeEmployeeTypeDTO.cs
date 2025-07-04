using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class ItemTypeEmployeeTypeDTO
    {
        public int ItemTypeId { get; set; }
        public string? ItemTypeName { get; set; }
        public int EmployeeTypeId { get; set; }
        public string? EmployeeTypeName { get; set; }
    }
    
    public class CreateItemTypeEmployeeTypeDTO
    {
        [Required]
        public int ItemTypeId { get; set; }
        
        [Required]
        public int EmployeeTypeId { get; set; }
    }
}
