using FluentAssertions;
using GoodHamburguer.Blazor.Services.Api.Menu;
using GoodHamburguer.Blazor.Services.Api.Orders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GoodHamburguer.Blazor.Tests;

public class ProgramBootstrapTests
{
    [Fact]
    public async Task BuildApp_ShouldRegisterApiClients()
    {
        var app = Program.BuildApp(["--Api:BaseUrl=https://api.goodhamburguer.local"], Environments.Development, mapStaticAssets: false);

        try
        {
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<IMenuApiClient>().Should().NotBeNull();
            scope.ServiceProvider.GetRequiredService<IOrderApiClient>().Should().NotBeNull();
        }
        finally
        {
            await app.DisposeAsync();
        }
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Production")]
    public async Task BuildApp_ShouldSupportExpectedEnvironment(string environment)
    {
        var app = Program.BuildApp(["--Api:BaseUrl=https://api.goodhamburguer.local"], environment, mapStaticAssets: false);

        try
        {
            app.Environment.EnvironmentName.Should().Be(environment);
        }
        finally
        {
            await app.DisposeAsync();
        }
    }

    [Fact]
    public async Task BuildApp_ShouldMapStaticAssets_WhenManifestPathIsProvided()
    {
        var staticAssetsManifestPath = Path.Combine(AppContext.BaseDirectory, "GoodHamburguer.Blazor.staticwebassets.endpoints.json");
        File.Exists(staticAssetsManifestPath).Should().BeTrue();

        var app = Program.BuildApp(
            ["--Api:BaseUrl=https://api.goodhamburguer.local"],
            Environments.Development,
            mapStaticAssets: true,
            staticAssetsManifestPath: staticAssetsManifestPath);

        try
        {
            app.Environment.IsDevelopment().Should().BeTrue();
        }
        finally
        {
            await app.DisposeAsync();
        }
    }

    [Fact]
    public async Task BuildApp_ShouldMapStaticAssets_WithDefaultManifestLookup()
    {
        var sourceManifestPath = Path.Combine(AppContext.BaseDirectory, "GoodHamburguer.Blazor.staticwebassets.endpoints.json");
        var defaultManifestPath = Path.Combine(AppContext.BaseDirectory, "testhost.staticwebassets.endpoints.json");
        File.Exists(sourceManifestPath).Should().BeTrue();

        File.Copy(sourceManifestPath, defaultManifestPath, overwrite: true);

        var app = Program.BuildApp(
            ["--Api:BaseUrl=https://api.goodhamburguer.local"],
            Environments.Development,
            mapStaticAssets: true);

        try
        {
            app.Environment.IsDevelopment().Should().BeTrue();
        }
        finally
        {
            await app.DisposeAsync();
            File.Delete(defaultManifestPath);
        }
    }
}
