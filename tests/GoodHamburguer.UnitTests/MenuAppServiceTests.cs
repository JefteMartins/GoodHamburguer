using FluentAssertions;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.Menu.Services;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.UnitTests;

public class MenuAppServiceTests
{
    [Fact]
    public async Task GetMenuAsync_ShouldGroupItemsByCategoryAndSortByName()
    {
        var service = new MenuAppService(new StubMenuQueryService(
            new MenuCatalog(
            [
                new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m),
                new MenuItem("sandwich-x-bacon", "X Bacon", MenuCategory.Sandwiches, 7.00m),
                new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m)
            ])));

        var response = await service.GetMenuAsync();

        response.Should().HaveCount(2);
        response.Select(group => group.Code)
            .Should()
            .Equal("drinks", "sandwiches");
        response.Single(group => group.Code == "sandwiches").Items.Select(item => item.Name)
            .Should()
            .Equal("X Bacon", "X Burger");
        response.Single(group => group.Code == "sandwiches").Items.Select(item => item.Code)
            .Should()
            .Equal("sandwich-x-bacon", "sandwich-x-burger");
    }

    private sealed class StubMenuQueryService(MenuCatalog catalog) : IMenuQueryService
    {
        public Task<MenuCatalog> GetMenuCatalogAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(catalog);
        }
    }
}
