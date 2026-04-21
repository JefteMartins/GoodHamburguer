namespace GoodHamburguer.Infrastructure.Configuration;

internal sealed class DatabaseSettings
{
    public string Host { get; init; } = "localhost";

    public int Port { get; init; } = 3306;

    public string Name { get; init; } = "goodhamburguer";

    public string User { get; init; } = "goodhamburguer";

    public string Password { get; init; } = string.Empty;
}
