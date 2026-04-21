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
            new MenuItem("X Bacon", MenuCategory.Sandwiches, 7.00m),
            new MenuItem("Refrigerante", MenuCategory.Drinks, 2.50m),
            new MenuItem("Batata frita", MenuCategory.Sides, 2.00m),
            new MenuItem("X Burger", MenuCategory.Sandwiches, 5.00m)
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
}
