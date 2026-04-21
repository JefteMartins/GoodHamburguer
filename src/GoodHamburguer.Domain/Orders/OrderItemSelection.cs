namespace GoodHamburguer.Domain.Orders;

public sealed class OrderItemSelection
{
    public OrderItemSelection(string itemCode)
    {
        if (string.IsNullOrWhiteSpace(itemCode))
        {
            throw new ArgumentException("Order item code cannot be empty.", nameof(itemCode));
        }

        ItemCode = itemCode.Trim();
    }

    public string ItemCode { get; }
}
