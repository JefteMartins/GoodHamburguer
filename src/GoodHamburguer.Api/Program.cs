using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using GoodHamburguer.Api.Swagger;
using GoodHamburguer.Application;
using GoodHamburguer.Infrastructure;
using GoodHamburguer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GoodHamburguer.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();

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
        app.MapControllers();
        await app.RunAsync();
    }
}
