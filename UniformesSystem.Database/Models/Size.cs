namespace UniformesSystem.Database.Models;

public enum SizeSystem
{
    Mexican,
    American,
    European,
    OneSize
}

public class Size
{
    public int SizeId { get; set; }
    public string Value { get; set; } = string.Empty;
    public SizeSystem System { get; set; }
    
    public ICollection<Item> Items { get; set; } = new List<Item>();
}
