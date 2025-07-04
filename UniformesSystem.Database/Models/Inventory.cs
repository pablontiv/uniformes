namespace UniformesSystem.Database.Models;

public class Inventory
{
    public int ItemId { get; set; }
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    
    public Item Item { get; set; } = null!;
}
