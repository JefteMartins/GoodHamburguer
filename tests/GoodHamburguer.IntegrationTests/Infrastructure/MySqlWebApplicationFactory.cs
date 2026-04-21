using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySqlConnector;
using Testcontainers.MySql;

namespace GoodHamburguer.IntegrationTests.Infrastructure;

public sealed class MySqlWebApplicationFactory : WebApplicationFactory<GoodHamburguer.Api.Program>, IAsyncLifetime
{
    private readonly MySqlContainer _container = new MySqlBuilder("mysql:8.4")
        .WithDatabase("goodhamburguer_tests")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<GoodHamburguerDbContext>>();
            services.RemoveAll<GoodHamburguerDbContext>();

            services.AddDbContext<GoodHamburguerDbContext>(options =>
            {
                options.UseMySql(
                    _container.GetConnectionString(),
                    new MySqlServerVersion(new Version(8, 4, 0)),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly(typeof(GoodHamburguerDbContext).Assembly.FullName);
                        mySqlOptions.EnableRetryOnFailure();
                    });
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        });
    }

    public HttpClient CreateApiClient()
    {
        return CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var expectedConnectionString = _container.GetConnectionString();

        await WaitForDatabaseAsync(expectedConnectionString);

        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GoodHamburguerDbContext>();
        var dbContextConnectionString = dbContext.Database.GetConnectionString();

        await WaitForDatabaseAsync(dbContextConnectionString!);

        var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await initializer.InitializeAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Dispose();
        await _container.DisposeAsync();
    }

    private static async Task WaitForDatabaseAsync(string connectionString)
    {
        var timeoutAt = DateTimeOffset.UtcNow.AddMinutes(2);
        Exception? lastException = null;

        while (DateTimeOffset.UtcNow < timeoutAt)
        {
            try
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                return;
            }
            catch (Exception exception)
            {
                lastException = exception;
                await Task.Delay(1000);
            }
        }

        throw new InvalidOperationException(
            $"Timed out waiting for MySQL test container. Connection string: {connectionString}",
            lastException);
    }
}
