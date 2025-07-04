namespace UniformesSystem.Database.Models;

public class EmployeeType
{
    public int EmployeeTypeId { get; set; }
    public string Type { get; set; } = string.Empty;
    
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
