namespace GoodHamburguer.Blazor.Services.Api.Orders;

public interface IOrderApiClient
{
    Task<IReadOnlyList<OrderSummaryDto>> ListOrdersAsync(
        CancellationToken cancellationToken = default);

    Task<OrderSummaryDto> CreateOrderAsync(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken = default);

    Task<OrderSummaryDto> GetOrderByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<OrderSummaryDto> UpdateOrderAsync(
        Guid id,
        UpdateOrderRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteOrderAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
