namespace GoodHamburguer.Application.OperationalLogs.Contracts;

public sealed class OperationalLogQueryFilter
{
    public OperationalLogType? Type { get; init; }

    public DateTimeOffset? From { get; init; }

    public DateTimeOffset? To { get; init; }

    public int Limit { get; init; } = 100;
}
