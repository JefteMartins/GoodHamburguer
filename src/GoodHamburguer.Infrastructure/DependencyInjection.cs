using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GoodHamburguer.Application.Menu.Abstractions;
using GoodHamburguer.Application.OperationalLogs.Abstractions;
using GoodHamburguer.Application.Orders.Abstractions;
using GoodHamburguer.Infrastructure.Configuration;
using GoodHamburguer.Infrastructure.Menu;
using GoodHamburguer.Infrastructure.OperationalLogs;
using GoodHamburguer.Infrastructure.Orders;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GoodHamburguer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection("Database").Get<DatabaseSettings>()
            ?? new DatabaseSettings();

        if (string.IsNullOrWhiteSpace(databaseSettings.Host)
            || string.IsNullOrWhiteSpace(databaseSettings.Name)
            || string.IsNullOrWhiteSpace(databaseSettings.User))
        {
            throw new InvalidOperationException("Database configuration is incomplete.");
        }

        var connectionString = new MySqlConnectionStringBuilder
        {
            Server = databaseSettings.Host,
            Port = (uint)databaseSettings.Port,
            Database = databaseSettings.Name,
            UserID = databaseSettings.User,
            Password = databaseSettings.Password
        }.ConnectionString;

        services.AddDbContext<GoodHamburguerDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 4, 0)),
                mySqlOptions =>
                {
                    mySqlOptions.MigrationsAssembly(typeof(GoodHamburguerDbContext).Assembly.FullName);
                    mySqlOptions.EnableRetryOnFailure();
                });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<IMenuQueryService, PersistedMenuQueryService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOperationalLogRepository, OperationalLogRepository>();
        services.AddScoped<IDatabaseInitializer, GoodHamburguerDbInitializer>();

        return services;
    }
}
