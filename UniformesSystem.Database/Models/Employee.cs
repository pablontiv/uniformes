namespace UniformesSystem.Database.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GroupId { get; set; }
    
    public Group Group { get; set; } = null!;
    public ICollection<WarehouseMovement> WarehouseMovements { get; set; } = new List<WarehouseMovement>();
}
