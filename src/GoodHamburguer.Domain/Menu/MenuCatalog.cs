namespace GoodHamburguer.Domain.Menu;

public sealed class MenuCatalog
{
    private readonly IReadOnlyCollection<MenuItem> _items;
    private readonly Dictionary<string, MenuItem> _itemsByCode;

    public MenuCatalog(IEnumerable<MenuItem> items)
    {
        _items = items?.ToArray() ?? throw new ArgumentNullException(nameof(items));
        _itemsByCode = _items.ToDictionary(item => item.Code, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyCollection<MenuItem> Items => _items;

    public MenuItem? FindByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        return _itemsByCode.TryGetValue(code.Trim(), out var item) ? item : null;
    }

    public IReadOnlyCollection<MenuCategoryGroup> GroupByCategory()
    {
        return _items
            .GroupBy(item => item.Category)
            .OrderBy(group => group.Key.Code)
            .Select(group => new MenuCategoryGroup(group.Key, group.OrderBy(item => item.Name).ToArray()))
            .ToArray();
    }
}
