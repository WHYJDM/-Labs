namespace Lab6.Models;

/// <summary>
/// Represents a drink product linked to a manufacturer.
/// </summary>
public class Drink
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int ManufacturerId { get; set; }
}
