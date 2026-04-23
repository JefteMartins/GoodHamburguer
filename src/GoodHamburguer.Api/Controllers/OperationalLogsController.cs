using Asp.Versioning;
using GoodHamburguer.Application.OperationalLogs.Contracts;
using GoodHamburguer.Application.OperationalLogs.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operational-logs")]
public sealed class OperationalLogsController(IOperationalLogService operationalLogService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<OperationalLogResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OperationalLogResponse>>> List(
        [FromQuery] string? type,
        [FromQuery] DateTimeOffset? from,
        [FromQuery] DateTimeOffset? to,
        [FromQuery] int? limit,
        CancellationToken cancellationToken)
    {
        var parsedType = ParseType(type);
        var logs = await operationalLogService.ListAsync(
            new OperationalLogQueryFilter
            {
                Type = parsedType,
                From = from,
                To = to,
                Limit = limit ?? 100
            },
            cancellationToken);

        return Ok(logs);
    }

    private static OperationalLogType? ParseType(string? type)
    {
        return type?.Trim().ToLowerInvariant() switch
        {
            null or "" => null,
            "application" => OperationalLogType.Application,
            "error" => OperationalLogType.Error,
            _ => null
        };
    }
}
