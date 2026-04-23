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
        createdOrder.Subtotal.Should().Be(7.50m);
        createdOrder.Discount.Should().Be(0m);
        createdOrder.Total.Should().Be(7.50m);

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
        updatedOrder.Subtotal.Should().Be(2.00m);
        updatedOrder.Discount.Should().Be(0m);
        updatedOrder.Total.Should().Be(2.00m);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WithLocationHeader()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            sideItemCode = "side-fries",
            drinkItemCode = "drink-soft-drink"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var createdOrder = await response.Content.ReadFromJsonAsync<OrderContract>();
        createdOrder.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().EndWith($"/api/v1/orders/{createdOrder!.Id}");
    }

    [Fact]
    public async Task Create_ShouldReturnCalculatedDiscount_WhenOrderHasFullCombo()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            sideItemCode = "side-fries",
            drinkItemCode = "drink-soft-drink"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var order = await response.Content.ReadFromJsonAsync<OrderContract>();
        order.Should().NotBeNull();
        order!.Subtotal.Should().Be(9.50m);
        order.Discount.Should().Be(1.90m);
        order.Total.Should().Be(7.60m);
        order.CreatedAtUtc.Should().NotBe(default);
        order.UpdatedAtUtc.Should().Be(order.CreatedAtUtc);
    }

    [Fact]
    public async Task GetById_ShouldReturnExistingOrder()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            sideItemCode = "side-fries",
            drinkItemCode = "drink-soft-drink"
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();
        createdOrder.Should().NotBeNull();
        var expectedCreatedAtUtc = TruncateToMicroseconds(createdOrder!.CreatedAtUtc);
        var expectedUpdatedAtUtc = TruncateToMicroseconds(createdOrder.UpdatedAtUtc);

        var getResponse = await _client.GetAsync($"/api/v1/orders/{createdOrder.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var order = await getResponse.Content.ReadFromJsonAsync<OrderContract>();
        order.Should().NotBeNull();
        order!.Id.Should().Be(createdOrder.Id);
        order.SandwichItemCode.Should().Be("sandwich-x-burger");
        order.SideItemCode.Should().Be("side-fries");
        order.DrinkItemCode.Should().Be("drink-soft-drink");
        order.Subtotal.Should().Be(9.50m);
        order.Discount.Should().Be(1.90m);
        order.Total.Should().Be(7.60m);
        order.CreatedAtUtc.Should().Be(expectedCreatedAtUtc);
        order.UpdatedAtUtc.Should().Be(expectedUpdatedAtUtc);
    }

    [Fact]
    public async Task List_ShouldReturnPersistedOrders()
    {
        var initialListResponse = await _client.GetAsync("/api/v1/orders");
        initialListResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var initialOrders = await initialListResponse.Content.ReadFromJsonAsync<List<OrderContract>>();
        initialOrders.Should().NotBeNull();

        var firstCreateResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            drinkItemCode = "drink-soft-drink"
        });

        firstCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var firstCreatedOrder = await firstCreateResponse.Content.ReadFromJsonAsync<OrderContract>();
        firstCreatedOrder.Should().NotBeNull();
        var firstExpectedCreatedAtUtc = TruncateToMicroseconds(firstCreatedOrder!.CreatedAtUtc);
        var firstExpectedUpdatedAtUtc = TruncateToMicroseconds(firstCreatedOrder.UpdatedAtUtc);

        var secondCreateResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-egg",
            sideItemCode = "side-fries"
        });

        secondCreateResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondCreatedOrder = await secondCreateResponse.Content.ReadFromJsonAsync<OrderContract>();
        secondCreatedOrder.Should().NotBeNull();
        var secondExpectedCreatedAtUtc = TruncateToMicroseconds(secondCreatedOrder!.CreatedAtUtc);
        var secondExpectedUpdatedAtUtc = TruncateToMicroseconds(secondCreatedOrder.UpdatedAtUtc);

        var listResponse = await _client.GetAsync("/api/v1/orders");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var orders = await listResponse.Content.ReadFromJsonAsync<List<OrderContract>>();
        orders.Should().NotBeNull();
        orders.Should().HaveCount(initialOrders!.Count + 2);
        orders.Should().ContainSingle(order =>
            order.Id == firstCreatedOrder.Id
            && order.SandwichItemCode == "sandwich-x-burger"
            && order.SideItemCode == null
            && order.DrinkItemCode == "drink-soft-drink"
            && order.Subtotal == 7.50m
            && order.Discount == 0m
            && order.Total == 7.50m
            && order.CreatedAtUtc == firstExpectedCreatedAtUtc
            && order.UpdatedAtUtc == firstExpectedUpdatedAtUtc);
        orders.Should().ContainSingle(order =>
            order.Id == secondCreatedOrder.Id
            && order.SandwichItemCode == "sandwich-x-egg"
            && order.SideItemCode == "side-fries"
            && order.DrinkItemCode == null
            && order.Subtotal == 6.50m
            && order.Discount == 0m
            && order.Total == 6.50m
            && order.CreatedAtUtc == secondExpectedCreatedAtUtc
            && order.UpdatedAtUtc == secondExpectedUpdatedAtUtc);
    }

    [Fact]
    public async Task Delete_ShouldRemoveExistingOrder()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger"
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdOrder = await createResponse.Content.ReadFromJsonAsync<OrderContract>();
        createdOrder.Should().NotBeNull();

        var deleteResponse = await _client.DeleteAsync($"/api/v1/orders/{createdOrder!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        deleteResponse.Content.Headers.ContentLength.Should().Be(0);

        var getResponse = await _client.GetAsync($"/api/v1/orders/{createdOrder.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        var response = await _client.GetAsync($"/api/v1/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        var response = await _client.DeleteAsync($"/api/v1/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public sealed class OrderContract
    {
        public required Guid Id { get; init; }

        public string? SandwichItemCode { get; init; }

        public string? SideItemCode { get; init; }

        public string? DrinkItemCode { get; init; }

        public decimal Subtotal { get; init; }

        public decimal Discount { get; init; }

        public decimal Total { get; init; }

        public DateTimeOffset CreatedAtUtc { get; init; }

        public DateTimeOffset UpdatedAtUtc { get; init; }
    }

    public sealed class ValidationProblemContract
    {
        public required IDictionary<string, string[]> Errors { get; init; }
    }

    private static DateTimeOffset TruncateToMicroseconds(DateTimeOffset value)
    {
        const long ticksPerMicrosecond = 10;
        return new DateTimeOffset(value.Ticks - (value.Ticks % ticksPerMicrosecond), value.Offset);
    }
}
