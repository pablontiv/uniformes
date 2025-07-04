namespace UniformesSystem.Database.Models;

public class WarehouseMovement
{
    public int WarehouseMovementId { get; set; }
    public DateTime Date { get; set; }
    public MovementType MovementType { get; set; }
    public string? Notes { get; set; }
    public int? EmployeeId { get; set; }
    
    public Employee? Employee { get; set; }
    public ICollection<WarehouseMovementDetail> Details { get; set; } = new List<WarehouseMovementDetail>();
}
