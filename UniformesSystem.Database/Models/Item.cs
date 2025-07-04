namespace UniformesSystem.Database.Models;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ItemTypeId { get; set; }
    public int SizeId { get; set; }
    
    public ItemType ItemType { get; set; } = null!;
    public Size Size { get; set; } = null!;
    
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    public ICollection<WarehouseMovementDetail> WarehouseMovementDetails { get; set; } = new List<WarehouseMovementDetail>();
}
