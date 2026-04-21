using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Domain.Orders;

public sealed class OrderItemSelection
{
    public OrderItemSelection(string name, MenuCategory category, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Order item name cannot be empty.", nameof(name));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Order item price cannot be negative.");
        }

        Name = name.Trim();
        Category = category ?? throw new ArgumentNullException(nameof(category));
        UnitPrice = unitPrice;
    }

    public string Name { get; }

    public MenuCategory Category { get; }

    public decimal UnitPrice { get; }
}
