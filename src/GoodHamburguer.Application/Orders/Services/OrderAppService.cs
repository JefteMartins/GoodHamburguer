using System.Diagnostics;
using FluentValidation;
using GoodHamburguer.Application.Common.Telemetry;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Domain.Menu;
using OpenTelemetry.Trace;

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
        var startTime = Stopwatch.GetTimestamp();
        using var activity = ApplicationTelemetry.ActivitySource.StartActivity("OrderAppService.Create");
        
        try
        {
            await createOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

            var orderId = Guid.NewGuid();
            activity?.SetTag("goodhamburguer.order.id", orderId);

            var order = orderDraftingService.CreateOrder(orderId, request, DateTimeOffset.UtcNow);
            await orderRepository.AddAsync(order, cancellationToken);

            var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
            var response = Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));

            activity?.SetStatus(ActivityStatusCode.Ok);
            ApplicationTelemetry.OrdersCreatedCounter.Add(1);
            return response;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
        finally
        {
            var duration = Stopwatch.GetElapsedTime(startTime);
            ApplicationTelemetry.OrderProcessingDuration.Record(duration.TotalSeconds, new TagList { { "operation", "create" } });
        }
    }

    public async Task<IReadOnlyList<OrderResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        using var activity = ApplicationTelemetry.ActivitySource.StartActivity("OrderAppService.List");

        var orders = await orderRepository.ListAsync(cancellationToken);
        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        var menuItemsByCode = menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase);

        activity?.SetTag("goodhamburguer.order.count", orders.Count);

        return orders
            .Select(order => Map(order, menuItemsByCode))
            .ToArray();
    }

    public async Task<OrderResponse> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        using var activity = ApplicationTelemetry.ActivitySource.StartActivity("OrderAppService.GetById");
        activity?.SetTag("goodhamburguer.order.id", orderId);

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

        var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
        return Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));
    }

    public async Task<OrderResponse> UpdateAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var startTime = Stopwatch.GetTimestamp();
        using var activity = ApplicationTelemetry.ActivitySource.StartActivity("OrderAppService.Update");
        activity?.SetTag("goodhamburguer.order.id", orderId);

        try
        {
            await updateOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

            var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
                ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

            orderDraftingService.UpdateOrder(order, request, DateTimeOffset.UtcNow);
            await orderRepository.UpdateAsync(order, cancellationToken);

            var menuCatalog = await menuQueryService.GetMenuCatalogAsync(cancellationToken);
            var response = Map(order, menuCatalog.Items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase));

            activity?.SetStatus(ActivityStatusCode.Ok);
            ApplicationTelemetry.OrdersUpdatedCounter.Add(1);
            return response;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
        finally
        {
            var duration = Stopwatch.GetElapsedTime(startTime);
            ApplicationTelemetry.OrderProcessingDuration.Record(duration.TotalSeconds, new TagList { { "operation", "update" } });
        }
    }

    public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var startTime = Stopwatch.GetTimestamp();
        using var activity = ApplicationTelemetry.ActivitySource.StartActivity("OrderAppService.Delete");
        activity?.SetTag("goodhamburguer.order.id", orderId);

        try
        {
            var wasDeleted = await orderRepository.DeleteAsync(orderId, cancellationToken);
            if (!wasDeleted)
            {
                throw new KeyNotFoundException($"Order '{orderId}' was not found.");
            }

            activity?.SetStatus(ActivityStatusCode.Ok);
            ApplicationTelemetry.OrdersDeletedCounter.Add(1);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            throw;
        }
        finally
        {
            var duration = Stopwatch.GetElapsedTime(startTime);
            ApplicationTelemetry.OrderProcessingDuration.Record(duration.TotalSeconds, new TagList { { "operation", "delete" } });
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
