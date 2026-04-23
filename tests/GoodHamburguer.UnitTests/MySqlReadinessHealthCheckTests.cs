using FluentAssertions;
using GoodHamburguer.Api.HealthChecks;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GoodHamburguer.UnitTests;

public sealed class MySqlReadinessHealthCheckTests
{
    [Fact]
    public async Task CheckHealthAsync_ShouldReturnHealthy_WhenDatabaseIsReachable()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GoodHamburguerDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var dbContext = new GoodHamburguerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        var sut = new MySqlReadinessHealthCheck(dbContext);

        var result = await sut.CheckHealthAsync(new HealthCheckContext());

        result.Status.Should().Be(HealthStatus.Healthy);
        result.Description.Should().Be("MySQL is reachable.");
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenDatabaseCheckThrows()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<GoodHamburguerDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new GoodHamburguerDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.DisposeAsync();

        var sut = new MySqlReadinessHealthCheck(dbContext);

        var result = await sut.CheckHealthAsync(new HealthCheckContext());

        result.Status.Should().Be(HealthStatus.Unhealthy);
        result.Description.Should().Be("MySQL readiness probe failed.");
        result.Exception.Should().NotBeNull();
    }
}
