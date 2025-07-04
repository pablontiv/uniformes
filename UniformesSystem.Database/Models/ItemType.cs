namespace UniformesSystem.Database.Models;

public class ItemType
{
    public int ItemTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsUniversal { get; set; }
    
    public ICollection<ItemTypeEmployeeType> ItemTypeEmployeeTypes { get; set; } = new List<ItemTypeEmployeeType>();
    public ICollection<Item> Items { get; set; } = new List<Item>();
}
