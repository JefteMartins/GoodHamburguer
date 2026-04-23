using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class OrderApiClient(IHttpClientFactory httpClientFactory) : IOrderApiClient
{
    public async Task<IReadOnlyList<OrderSummaryDto>> ListOrdersAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.GetAsync("orders", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw await CreateProblemExceptionAsync(response, cancellationToken);
        }

        return await ReadOrderListAsync(response, cancellationToken);
    }

    public async Task<OrderSummaryDto> CreateOrderAsync(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.PostAsJsonAsync("orders", request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity)
        {
            throw await CreateValidationExceptionAsync(response, cancellationToken);
        }

        response.EnsureSuccessStatusCode();

        return await ReadRequiredOrderAsync(response, cancellationToken);
    }

    public async Task<OrderSummaryDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.GetAsync($"orders/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new OrderApiNotFoundException("The requested order was not found.");
        }

        response.EnsureSuccessStatusCode();

        return await ReadRequiredOrderAsync(response, cancellationToken);
    }

    public async Task<OrderSummaryDto> UpdateOrderAsync(
        Guid id,
        UpdateOrderRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.PutAsJsonAsync($"orders/{id}", request, cancellationToken);

        if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity)
        {
            throw await CreateValidationExceptionAsync(response, cancellationToken);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new OrderApiNotFoundException("The requested order was not found.");
        }

        response.EnsureSuccessStatusCode();

        return await ReadRequiredOrderAsync(response, cancellationToken);
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(Program.ApiHttpClientName);
        using var response = await client.DeleteAsync($"orders/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new OrderApiNotFoundException("The requested order was not found.");
        }

        response.EnsureSuccessStatusCode();
    }

    private static async Task<OrderApiValidationException> CreateValidationExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ValidationProblemDetails? details;
        try
        {
            details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
        }
        catch (JsonException)
        {
            details = null;
        }

        var errors = details?.Errors?.ToDictionary(pair => pair.Key, pair => pair.Value) ??
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        var message = !string.IsNullOrWhiteSpace(details?.Detail)
            ? details.Detail
            : "The order request contains invalid values.";

        return new OrderApiValidationException(errors, message);
    }

    private static async Task<OrderApiProblemException> CreateProblemExceptionAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ProblemDetails? details;
        try
        {
            details = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);
        }
        catch (JsonException)
        {
            details = null;
        }

        var message = !string.IsNullOrWhiteSpace(details?.Detail)
            ? details.Detail
            : !string.IsNullOrWhiteSpace(details?.Title)
                ? details.Title
                : "The order request could not be completed.";

        return new OrderApiProblemException(message);
    }

    private static async Task<IReadOnlyList<OrderSummaryDto>> ReadOrderListAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.Content.Headers.ContentLength is 0)
        {
            return [];
        }

        try
        {
            var orders = await response.Content.ReadFromJsonAsync<IReadOnlyList<OrderSummaryDto>>(cancellationToken: cancellationToken);
            return orders ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static async Task<OrderSummaryDto> ReadRequiredOrderAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.Content.Headers.ContentLength is 0)
        {
            throw new InvalidOperationException("The API returned an empty order payload.");
        }

        try
        {
            var order = await response.Content.ReadFromJsonAsync<OrderSummaryDto>(cancellationToken: cancellationToken);
            return order ?? throw new InvalidOperationException("The API returned an empty order payload.");
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("The API returned an empty order payload.");
        }
    }
}
