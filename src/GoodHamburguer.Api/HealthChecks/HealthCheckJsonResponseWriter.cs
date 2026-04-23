using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GoodHamburguer.Api.HealthChecks;

public static class HealthCheckJsonResponseWriter
{
    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var payload = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            entries = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration
                })
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
