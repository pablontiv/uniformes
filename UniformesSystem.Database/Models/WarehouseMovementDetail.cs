namespace UniformesSystem.Database.Models;

public class WarehouseMovementDetail
{
    public int WarehouseMovementDetailId { get; set; }
    public int WarehouseMovementId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    
    public WarehouseMovement WarehouseMovement { get; set; } = null!;
    public Item Item { get; set; } = null!;
}
