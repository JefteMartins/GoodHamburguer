namespace GoodHamburguer.Infrastructure.Persistence.Entities;

public sealed class OperationalLogEntity
{
    public long Id { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;

    public string Method { get; set; } = string.Empty;

    public string? CorrelationId { get; set; }

    public string Payload { get; set; } = "{}";

    public int? StatusCode { get; set; }

    public string? ExceptionType { get; set; }

    public string? ErrorMessage { get; set; }
}
