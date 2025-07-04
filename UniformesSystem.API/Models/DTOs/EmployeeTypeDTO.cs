using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class EmployeeTypeDTO
    {
        public int EmployeeTypeId { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
