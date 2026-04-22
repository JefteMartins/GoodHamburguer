using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GoodHamburguer.Api.HealthChecks;

public sealed class MySqlReadinessHealthCheck(GoodHamburguerDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("MySQL is reachable.")
                : HealthCheckResult.Unhealthy("MySQL is not reachable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("MySQL readiness probe failed.", exception);
        }
    }
}
