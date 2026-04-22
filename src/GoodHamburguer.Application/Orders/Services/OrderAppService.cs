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
        return Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));
    }

    public async Task<IReadOnlyList<OrderResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var orders = await orderRepository.ListAsync(cancellationToken);
        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        var menuItemsByCode = menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase);

        return orders
            .Select(order => Map(order, menuItemsByCode))
            .ToArray();
    }

    public async Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));
    }

    public async Task<OrderResponse> UpdateAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await updateOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

        orderDraftingService.UpdateOrder(order, request, DateTimeOffset.UtcNow);
        await orderRepository.UpdateAsync(order, cancellationToken);

        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));
    }

    public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var wasDeleted = await orderRepository.DeleteAsync(orderId, cancellationToken);
        if (!wasDeleted)
        {
            throw new KeyNotFoundException($"Order '{orderId}' was not found.");
        }
    }

    private static OrderResponse Map(
        Domain.Orders.Order order,
        IReadOnlyDictionary<string, MenuItem> menuItemsByCode)
    {
        var pricing = order.CalculatePricing(menuItemsByCode);

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
