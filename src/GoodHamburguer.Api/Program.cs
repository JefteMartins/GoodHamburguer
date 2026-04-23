using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using GoodHamburguer.Api.Exceptions;
using GoodHamburguer.Api.HealthChecks;
using GoodHamburguer.Api.OperationalLogs;
using GoodHamburguer.Api.Swagger;
using GoodHamburguer.Application;
using GoodHamburguer.Application.Common.Telemetry;
using GoodHamburguer.Infrastructure;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GoodHamburguer.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

        builder.Services.AddProblemDetails();
        builder.Services
            .AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

        builder.Services.AddScoped<GlobalExceptionFilter>();
        builder.Services.AddSingleton<IExceptionProblemDetailsMapper, KeyNotFoundExceptionProblemDetailsMapper>();
        builder.Services.AddSingleton<IExceptionProblemDetailsMapper, FluentValidationExceptionProblemDetailsMapper>();

        builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressMapClientErrors = false;
        });

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("GoodHamburguer.Api"))
            .WithTracing(tracing => tracing
                .AddSource(ApplicationTelemetry.ServiceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithMetrics(metrics => metrics
                .AddMeter(ApplicationTelemetry.ServiceName)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation());

        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
            .AddCheck<MySqlReadinessHealthCheck>("mysql", tags: ["ready"]);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (builder.Configuration.GetValue<bool>("Database:RunMigrationsOnly")
            || args.Contains("--migrate-only", StringComparer.OrdinalIgnoreCase))
        {
            await using var migrationScope = app.Services.CreateAsyncScope();
            var initializer = migrationScope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            await initializer.InitializeAsync();
            return;
        }

        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<OperationalLoggingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"GoodHamburguer API {description.GroupName.ToUpperInvariant()}");
            }
        });

        app.UseAuthorization();
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("live"),
            ResponseWriter = HealthCheckJsonResponseWriter.WriteAsync
        });
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("ready"),
            ResponseWriter = HealthCheckJsonResponseWriter.WriteAsync
        });
        app.MapControllers();
        await app.RunAsync();
    }
}
