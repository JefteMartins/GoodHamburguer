using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests.Orders;

public class OrderApiClientTests
{
    [Fact]
    public async Task ListOrdersAsync_ReturnsOrders()
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new[]
                {
                    new OrderSummaryDto
                    {
                        Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                        SandwichItemCode = "sandwich-x-burger",
                        SideItemCode = null,
                        DrinkItemCode = null,
                        Subtotal = 12m,
                        Discount = 0m,
                        Total = 12m,
                        CreatedAtUtc = DateTimeOffset.UtcNow,
                        UpdatedAtUtc = DateTimeOffset.UtcNow
                    }
                })
            });
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var result = await sut.ListOrdersAsync();

        result.Should().ContainSingle();
        capturedRequest!.Method.Should().Be(HttpMethod.Get);
        capturedRequest.RequestUri!.ToString().Should().Be("https://api.goodhamburguer.local/api/v1/orders");
    }

    [Fact]
    public async Task ListOrdersAsync_MapsProblemDetailsMessage()
    {
        var problem = new ProblemDetails
        {
            Title = "Orders unavailable",
            Detail = "The orders service is temporarily unavailable."
        };

        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = JsonContent.Create(problem)
            }));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.ListOrdersAsync();

        var exception = await act.Should().ThrowAsync<OrderApiProblemException>();
        exception.Which.Message.Should().Be("The orders service is temporarily unavailable.");
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsCreatedOrder()
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(_ =>
        {
            capturedRequest = _;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new OrderSummaryDto
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    SandwichItemCode = "sandwich-x-burger",
                    SideItemCode = "side-fries",
                    DrinkItemCode = "drink-soft-drink",
                    Subtotal = 21m,
                    Discount = 1m,
                    Total = 20m,
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                })
            });
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var request = new CreateOrderRequestDto
        {
            SandwichItemCode = "sandwich-x-burger"
        };

        var result = await sut.CreateOrderAsync(request);

        result.Id.Should().Be(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        result.Total.Should().Be(20m);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.Method.Should().Be(HttpMethod.Post);
        capturedRequest.RequestUri!.ToString().Should().Be("https://api.goodhamburguer.local/api/v1/orders");

        var postedRequest = await capturedRequest.Content!.ReadFromJsonAsync<CreateOrderRequestDto>();
        postedRequest!.SandwichItemCode.Should().Be("sandwich-x-burger");
    }

    [Fact]
    public async Task CreateOrderAsync_ThrowsValidationExceptionForProblemDetails()
    {
        var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["sandwichItemCode"] = ["The item code 'bad-item' was not found in the menu."]
        })
        {
            Detail = "The request payload contains invalid values."
        };

        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.UnprocessableEntity)
            {
                Content = JsonContent.Create(problemDetails)
            }));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.CreateOrderAsync(new CreateOrderRequestDto
        {
            SandwichItemCode = "bad-item"
        });

        var exception = await act.Should().ThrowAsync<OrderApiValidationException>();
        exception.Which.Errors.Should().ContainKey("sandwichItemCode");
    }

    [Fact]
    public async Task CreateOrderAsync_MapsBadRequestProblemDetailsAsValidation()
    {
        var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["drinkItemCode"] = ["The item code cannot be empty."]
        })
        {
            Detail = "The request payload contains invalid values."
        };

        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(problemDetails)
            }));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client =>
            {
                client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.CreateOrderAsync(new CreateOrderRequestDto());

        var exception = await act.Should().ThrowAsync<OrderApiValidationException>();
        exception.Which.Errors.Should().ContainKey("drinkItemCode");
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsOrderDetails()
    {
        HttpRequestMessage? capturedRequest = null;
        var orderId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new OrderSummaryDto
                {
                    Id = orderId,
                    SandwichItemCode = "sandwich-x-burger",
                    SideItemCode = "side-fries",
                    DrinkItemCode = "drink-soft-drink",
                    Subtotal = 21m,
                    Discount = 1m,
                    Total = 20m,
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                })
            });
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var result = await sut.GetOrderByIdAsync(orderId);

        result.Id.Should().Be(orderId);
        capturedRequest!.Method.Should().Be(HttpMethod.Get);
        capturedRequest.RequestUri!.ToString().Should().Be($"https://api.goodhamburguer.local/api/v1/orders/{orderId}");
    }

    [Fact]
    public async Task UpdateOrderAsync_SendsPutRequest()
    {
        HttpRequestMessage? capturedRequest = null;
        var orderId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new OrderSummaryDto
                {
                    Id = orderId,
                    SandwichItemCode = "sandwich-x-egg",
                    SideItemCode = null,
                    DrinkItemCode = null,
                    Subtotal = 10m,
                    Discount = 0m,
                    Total = 10m,
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                })
            });
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var result = await sut.UpdateOrderAsync(orderId, new UpdateOrderRequestDto
        {
            SandwichItemCode = "sandwich-x-egg"
        });

        result.SandwichItemCode.Should().Be("sandwich-x-egg");
        capturedRequest!.Method.Should().Be(HttpMethod.Put);
        capturedRequest.RequestUri!.ToString().Should().Be($"https://api.goodhamburguer.local/api/v1/orders/{orderId}");
    }

    [Fact]
    public async Task DeleteOrderAsync_SendsDeleteRequest()
    {
        HttpRequestMessage? capturedRequest = null;
        var orderId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));
        });

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        await sut.DeleteOrderAsync(orderId);

        capturedRequest!.Method.Should().Be(HttpMethod.Delete);
        capturedRequest.RequestUri!.ToString().Should().Be($"https://api.goodhamburguer.local/api/v1/orders/{orderId}");
    }

    [Fact]
    public async Task GetOrderByIdAsync_ThrowsNotFoundExceptionForMissingOrder()
    {
        var orderId = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.GetOrderByIdAsync(orderId);

        await act.Should().ThrowAsync<OrderApiNotFoundException>();
    }

    [Fact]
    public async Task UpdateOrderAsync_MapsValidationProblemDetails()
    {
        var orderId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var problemDetails = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["sideItemCode"] = ["The item code 'bad-side' was not found in the menu."]
        })
        {
            Detail = "The request payload contains invalid values."
        };

        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.UnprocessableEntity)
            {
                Content = JsonContent.Create(problemDetails)
            }));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.UpdateOrderAsync(orderId, new UpdateOrderRequestDto
        {
            SideItemCode = "bad-side"
        });

        var exception = await act.Should().ThrowAsync<OrderApiValidationException>();
        exception.Which.Errors.Should().ContainKey("sideItemCode");
    }

    [Fact]
    public async Task DeleteOrderAsync_ThrowsNotFoundExceptionForMissingOrder()
    {
        var orderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var handler = new StubHttpMessageHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));

        var services = new ServiceCollection();
        services.AddHttpClient(Program.ApiHttpClientName, client => client.BaseAddress = new Uri("https://api.goodhamburguer.local/api/v1/"))
            .ConfigurePrimaryHttpMessageHandler(() => handler);
        services.AddScoped<IOrderApiClient, OrderApiClient>();

        await using var provider = services.BuildServiceProvider().CreateAsyncScope();
        var sut = provider.ServiceProvider.GetRequiredService<IOrderApiClient>();

        var act = () => sut.DeleteOrderAsync(orderId);

        await act.Should().ThrowAsync<OrderApiNotFoundException>();
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => handler(request);
    }
}
