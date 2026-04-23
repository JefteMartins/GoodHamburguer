using FluentAssertions;
using GoodHamburguer.Domain.Menu;

namespace GoodHamburguer.UnitTests;

public class MenuCatalogTests
{
    [Fact]
    public void GroupByCategory_ShouldReturnItemsGroupedByCategory()
    {
        var catalog = new MenuCatalog(
        [
            new MenuItem("sandwich-x-bacon", "X Bacon", MenuCategory.Sandwiches, 7.00m),
            new MenuItem("drink-soft-drink", "Refrigerante", MenuCategory.Drinks, 2.50m),
            new MenuItem("side-fries", "Batata frita", MenuCategory.Sides, 2.00m),
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m)
        ]);

        var groups = catalog.GroupByCategory();

        groups.Should().HaveCount(3);
        groups.Select(group => group.Category.Code)
            .Should()
            .BeEquivalentTo(["drinks", "sandwiches", "sides"], options => options.WithStrictOrdering());

        var sandwiches = groups.Single(group => group.Category == MenuCategory.Sandwiches);
        sandwiches.Items.Select(item => item.Name)
            .Should()
            .Equal("X Bacon", "X Burger");
    }

    [Fact]
    public void FindByCode_ShouldReturnMatchingItemIgnoringCase()
    {
        var catalog = new MenuCatalog(
        [
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m)
        ]);

        var item = catalog.FindByCode("SANDWICH-X-BURGER");

        item.Should().NotBeNull();
        item!.Name.Should().Be("X Burger");
    }

    [Fact]
    public void FindByCode_ShouldReturnNull_WhenCodeIsEmpty()
    {
        var catalog = new MenuCatalog(
        [
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m)
        ]);

        catalog.FindByCode("   ").Should().BeNull();
    }

    [Fact]
    public void FindByCode_ShouldReturnNull_WhenCodeIsNotFound()
    {
        var catalog = new MenuCatalog(
        [
            new MenuItem("sandwich-x-burger", "X Burger", MenuCategory.Sandwiches, 5.00m)
        ]);

        catalog.FindByCode("sandwich-missing").Should().BeNull();
    }
}
