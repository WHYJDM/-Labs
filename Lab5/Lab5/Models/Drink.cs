namespace Lab5.Models;

public class Drink
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Manufacturer Manufacturer { get; set; } = new();

    public override string ToString() =>
        $"{Id}: {Name} ({Manufacturer.Name}, {Manufacturer.Country})";
}
