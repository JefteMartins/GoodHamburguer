using System.Text;
using System.Text.Json;
using GoodHamburguer.Application.OperationalLogs.Contracts;
using GoodHamburguer.Application.OperationalLogs.Services;

namespace GoodHamburguer.Api.OperationalLogs;

public sealed class OperationalLoggingMiddleware(
    RequestDelegate next,
    ILogger<OperationalLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, IOperationalLogService operationalLogService)
    {
        ArgumentNullException.ThrowIfNull(context);

        var request = context.Request;
        if (!ShouldCapture(request.Path))
        {
            await next(context);
            return;
        }

        var payload = await BuildPayloadAsync(request, context.RequestAborted);
        context.Items[OperationalLoggingHttpContextKeys.RequestPayload] = payload;

        await next(context);

        if (context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
        {
            return;
        }

        try
        {
            await operationalLogService.RecordAsync(
                new OperationalLogRecordRequest
                {
                    Type = OperationalLogType.Application,
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    Route = request.Path.Value ?? "/",
                    Method = request.Method,
                    CorrelationId = context.TraceIdentifier,
                    Payload = payload,
                    StatusCode = context.Response.StatusCode
                },
                context.RequestAborted);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to persist application operational log for {Method} {Path}.", request.Method, request.Path);
        }
    }

    private static bool ShouldCapture(PathString path)
    {
        return !path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase)
               && !path.StartsWithSegments("/health", StringComparison.OrdinalIgnoreCase)
               && !path.StartsWithSegments("/api/v1/operational-logs", StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<string> BuildPayloadAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        var body = await ReadBodyAsync(request, cancellationToken);
        var query = request.Query.ToDictionary(
            entry => entry.Key,
            entry => entry.Value.ToString(),
            StringComparer.OrdinalIgnoreCase);

        var payload = new
        {
            query,
            body
        };

        return JsonSerializer.Serialize(payload);
    }

    private static async Task<object?> ReadBodyAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        if (!HttpMethods.IsPost(request.Method)
            && !HttpMethods.IsPut(request.Method)
            && !HttpMethods.IsPatch(request.Method))
        {
            return null;
        }

        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var content = await reader.ReadToEndAsync(cancellationToken);
        request.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<JsonElement>(content);
        }
        catch (JsonException)
        {
            return content;
        }
    }
}
