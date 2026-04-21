namespace GoodHamburguer.Domain.Orders;

public sealed record OrderPricing(decimal Subtotal, decimal Discount, decimal Total);
