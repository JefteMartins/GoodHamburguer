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
        ValidateSlot(sandwich, MenuCategory.Sandwiches, nameof(sandwich));
        ValidateSlot(side, MenuCategory.Sides, nameof(side));
        ValidateSlot(drink, MenuCategory.Drinks, nameof(drink));

        Sandwich = sandwich;
        Side = side;
        Drink = drink;
    }

    private static void ValidateSlot(OrderItemSelection? item, MenuCategory expectedCategory, string slotName)
    {
        if (item is null)
        {
            return;
        }

        if (!item.Category.Equals(expectedCategory))
        {
            throw new ArgumentException(
                $"The item assigned to {slotName} must belong to category '{expectedCategory.Code}'.",
                slotName);
        }
    }
}
