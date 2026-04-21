namespace GoodHamburguer.Domain.Menu;

public sealed class MenuItem
{
    public MenuItem(string code, string name, MenuCategory category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Menu item code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Menu item name cannot be empty.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Menu item price cannot be negative.");
        }

        Code = code.Trim();
        Name = name.Trim();
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Price = price;
    }

    public string Code { get; }

    public string Name { get; }

    public MenuCategory Category { get; }

    public decimal Price { get; }
}
