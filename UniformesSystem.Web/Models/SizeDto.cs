using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UniformesSystem.Web.Models
{
    public enum SizeSystem
    {
        Mexican,
        American,
        European,
        OneSize
    }

    public class SizeDto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20, ErrorMessage = "Size value cannot exceed 20 characters.")]
        public string Value { get; set; } = string.Empty;
        
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SizeSystem System { get; set; }
    }
}
