using FluentAssertions;
using GoodHamburguer.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace GoodHamburguer.IntegrationTests;

public class MenuEndpointTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MenuEndpointTests(MySqlWebApplicationFactory factory)
    {
        _client = factory.CreateApiClient();
    }

    [Fact]
    public async Task GetMenu_ShouldReturnMenuGroupedByCategory()
    {
        var response = await _client.GetAsync("/api/v1/menu");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var menu = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<MenuCategoryContract>>();

        menu.Should().NotBeNull();

        var groups = menu!;

        groups.Should().HaveCount(3);
        groups.Select(group => group.Code)
            .Should()
            .Equal("drinks", "sandwiches", "sides");
        groups.Single(group => group.Code == "sandwiches").Items.Should().HaveCount(3);
    }

    public sealed class MenuCategoryContract
    {
        public required string Code { get; init; }

        public required string DisplayName { get; init; }

        public required IReadOnlyCollection<MenuItemContract> Items { get; init; }
    }

    public sealed class MenuItemContract
    {
        public required string Name { get; init; }

        public decimal Price { get; init; }
    }
}
