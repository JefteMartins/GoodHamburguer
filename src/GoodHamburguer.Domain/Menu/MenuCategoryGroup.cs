namespace GoodHamburguer.Domain.Menu;

public sealed class MenuCategoryGroup
{
    public MenuCategoryGroup(MenuCategory category, IReadOnlyCollection<MenuItem> items)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    public MenuCategory Category { get; }

    public IReadOnlyCollection<MenuItem> Items { get; }
}
