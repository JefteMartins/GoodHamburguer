using FluentAssertions;
using GoodHamburguer.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace GoodHamburguer.IntegrationTests;

public sealed class OrdersEndpointTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OrdersEndpointTests(MySqlWebApplicationFactory factory)
    {
        _client = factory.CreateApiClient();
    }

    [Fact]
    public async Task Create_ShouldReturnUnprocessableEntity_WhenItemCodeDoesNotExist()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-does-not-exist"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemContract>();

        problem.Should().NotBeNull();
        problem!.Errors.Should().ContainKey("sandwichItemCode");
    }

    [Fact]
    public async Task Update_ShouldReturnUnprocessableEntity_WhenItemCodeBelongsToWrongCategory()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger"
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();
        createdOrder.Should().NotBeNull();

        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/orders/{createdOrder!.Id}", new
        {
            sideItemCode = "sandwich-x-burger"
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var problem = await updateResponse.Content.ReadFromJsonAsync<ValidationProblemContract>();

        problem.Should().NotBeNull();
        problem!.Errors.Should().ContainKey("sideItemCode");
    }

    [Fact]
    public async Task CreateAndUpdate_ShouldAcceptValidItemCodes()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            drinkItemCode = "drink-soft-drink"
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();
        createdOrder.Should().NotBeNull();
        createdOrder!.SandwichItemCode.Should().Be("sandwich-x-burger");
        createdOrder.DrinkItemCode.Should().Be("drink-soft-drink");

        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/orders/{createdOrder.Id}", new
        {
            sideItemCode = "side-fries"
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedOrder = await updateResponse.Content.ReadFromJsonAsync<OrderContract>();
        updatedOrder.Should().NotBeNull();
        updatedOrder!.SandwichItemCode.Should().BeNull();
        updatedOrder.SideItemCode.Should().Be("side-fries");
        updatedOrder.DrinkItemCode.Should().BeNull();
    }

    public sealed class OrderContract
    {
        public required Guid Id { get; init; }

        public string? SandwichItemCode { get; init; }

        public string? SideItemCode { get; init; }

        public string? DrinkItemCode { get; init; }
    }

    public sealed class ValidationProblemContract
    {
        public required IDictionary<string, string[]> Errors { get; init; }
    }
}
