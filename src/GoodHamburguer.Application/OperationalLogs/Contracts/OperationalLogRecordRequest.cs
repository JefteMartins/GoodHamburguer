namespace GoodHamburguer.Application.OperationalLogs.Contracts;

public sealed class OperationalLogRecordRequest
{
    public required OperationalLogType Type { get; init; }

    public required DateTimeOffset CreatedAtUtc { get; init; }

    public required string Route { get; init; }

    public required string Method { get; init; }

    public string? CorrelationId { get; init; }

    public string Payload { get; init; } = "{}";

    public int? StatusCode { get; init; }

    public string? ExceptionType { get; init; }

    public string? ErrorMessage { get; init; }
}
