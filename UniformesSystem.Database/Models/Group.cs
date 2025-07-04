namespace UniformesSystem.Database.Models;

public class Group
{
    public int GroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int EmployeeTypeId { get; set; }
    
    public EmployeeType EmployeeType { get; set; } = null!;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
