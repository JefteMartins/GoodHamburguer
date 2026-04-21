namespace GoodHamburguer.Domain.Menu;

public sealed class MenuCategory : IEquatable<MenuCategory>
{
    public static readonly MenuCategory Sandwiches = new("sandwiches", "Sanduiches");
    public static readonly MenuCategory Sides = new("sides", "Acompanhamentos");
    public static readonly MenuCategory Drinks = new("drinks", "Bebidas");

    public MenuCategory(string code, string displayName)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Category code cannot be empty.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Category display name cannot be empty.", nameof(displayName));
        }

        Code = code.Trim().ToLowerInvariant();
        DisplayName = displayName.Trim();
    }

    public string Code { get; }

    public string DisplayName { get; }

    public bool Equals(MenuCategory? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Code, other.Code, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is MenuCategory other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Code);
    }
}
