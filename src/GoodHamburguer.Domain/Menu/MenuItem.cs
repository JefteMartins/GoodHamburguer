namespace GoodHamburguer.Domain.Menu;

public sealed class MenuItem
{
    public MenuItem(string name, MenuCategory category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Menu item name cannot be empty.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Menu item price cannot be negative.");
        }

        Name = name.Trim();
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Price = price;
    }

    public string Name { get; }

    public MenuCategory Category { get; }

    public decimal Price { get; }
}
