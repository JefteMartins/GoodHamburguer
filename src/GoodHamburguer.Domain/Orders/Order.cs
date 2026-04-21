using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Domain.Orders;

public sealed class Order
{
    private Order(
        Guid id,
        OrderItemSelection? sandwich,
        OrderItemSelection? side,
        OrderItemSelection? drink,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;

        ApplyItems(sandwich, side, drink);
    }

    public Guid Id { get; }

    public DateTimeOffset CreatedAtUtc { get; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public OrderItemSelection? Sandwich { get; private set; }

    public OrderItemSelection? Side { get; private set; }

    public OrderItemSelection? Drink { get; private set; }

    public static Order Create(
        Guid id,
        OrderItemSelection? sandwich,
        OrderItemSelection? side,
        OrderItemSelection? drink,
        DateTimeOffset createdAtUtc)
    {
        return new Order(id, sandwich, side, drink, createdAtUtc);
    }

    public void UpdateItems(
        OrderItemSelection? sandwich,
        OrderItemSelection? side,
        OrderItemSelection? drink,
        DateTimeOffset updatedAtUtc)
    {
        ApplyItems(sandwich, side, drink);
        UpdatedAtUtc = updatedAtUtc;
    }

    private void ApplyItems(
        OrderItemSelection? sandwich,
        OrderItemSelection? side,
        OrderItemSelection? drink)
    {
        Sandwich = sandwich;
        Side = side;
        Drink = drink;
    }

    public OrderPricing CalculatePricing(IReadOnlyDictionary<string, MenuItem> menuItemsByCode)
    {
        ArgumentNullException.ThrowIfNull(menuItemsByCode);

        var subtotal = GetSelectionPrice(menuItemsByCode, Sandwich)
            + GetSelectionPrice(menuItemsByCode, Side)
            + GetSelectionPrice(menuItemsByCode, Drink);

        var discount = Sandwich is not null && Side is not null && Drink is not null
            ? subtotal * 0.20m
            : 0m;

        return new OrderPricing(subtotal, discount, subtotal - discount);
    }

    private static decimal GetSelectionPrice(
        IReadOnlyDictionary<string, MenuItem> menuItemsByCode,
        OrderItemSelection? selection)
    {
        if (selection is null)
        {
            return 0m;
        }

        return menuItemsByCode[selection.ItemCode].Price;
    }
}
