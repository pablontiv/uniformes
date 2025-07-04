namespace UniformesSystem.Database.Models;

public class ItemTypeEmployeeType
{
    public int ItemTypeId { get; set; }
    public int EmployeeTypeId { get; set; }
    
    public ItemType ItemType { get; set; } = null!;
    public EmployeeType EmployeeType { get; set; } = null!;
}
