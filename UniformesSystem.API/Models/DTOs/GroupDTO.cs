using System.ComponentModel.DataAnnotations;

namespace UniformesSystem.API.Models.DTOs
{
    public class GroupDTO
    {
        public int GroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EmployeeTypeId { get; set; }
        public string? EmployeeTypeName { get; set; }
    }
}
