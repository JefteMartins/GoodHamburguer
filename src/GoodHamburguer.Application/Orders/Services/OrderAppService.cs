using FluentValidation;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.Application.Orders.Services;

public sealed class OrderAppService(
    IOrderRepository orderRepository,
    IOrderDraftingService orderDraftingService,
    IMenuQueryService menuQueryService,
    IValidator<CreateOrderRequest> createOrderValidator,
    IValidator<UpdateOrderRequest> updateOrderValidator) : IOrderAppService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await createOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = orderDraftingService.CreateOrder(Guid.NewGuid(), request, DateTimeOffset.UtcNow);
        await orderRepository.AddAsync(order, cancellationToken);

        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, menuCatalog);
    }

    public async Task<OrderResponse> UpdateAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await updateOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

        orderDraftingService.UpdateOrder(order, request, DateTimeOffset.UtcNow);
        await orderRepository.UpdateAsync(order, cancellationToken);

        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, menuCatalog);
    }

    private static OrderResponse Map(Domain.Orders.Order order, MenuCatalog menuCatalog)
    {
        var pricing = order.CalculatePricing(
            menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));

        return new OrderResponse
        {
            Id = order.Id,
            SandwichItemCode = order.Sandwich?.ItemCode,
            SideItemCode = order.Side?.ItemCode,
            DrinkItemCode = order.Drink?.ItemCode,
            Subtotal = pricing.Subtotal,
            Discount = pricing.Discount,
            Total = pricing.Total,
            CreatedAtUtc = order.CreatedAtUtc,
            UpdatedAtUtc = order.UpdatedAtUtc
        };
    }
}
