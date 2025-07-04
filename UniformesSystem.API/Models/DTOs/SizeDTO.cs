using System.ComponentModel.DataAnnotations;
using UniformesSystem.Database.Models;

namespace UniformesSystem.API.Models.DTOs
{
    public class SizeDTO
    {
        public int SizeId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string SizeName { get; set; } = string.Empty;
        
        [Required]
        public string SizeType { get; set; } = string.Empty;
    }
    
    public class CreateSizeDTO
    {
        [Required]
        [StringLength(50)]
        public string SizeName { get; set; } = string.Empty;
        
        [Required]
        [EnumDataType(typeof(SizeSystem))]
        public string SizeType { get; set; } = string.Empty;
    }
    
    public class UpdateSizeDTO
    {
        [Required]
        [StringLength(50)]
        public string SizeName { get; set; } = string.Empty;
        
        [Required]
        [EnumDataType(typeof(SizeSystem))]
        public string SizeType { get; set; } = string.Empty;
    }
}
