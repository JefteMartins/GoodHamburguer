using FluentAssertions;
using GoodHamburguer.Blazor.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GoodHamburguer.Blazor.Tests;

public class ApiOptionsBindingTests
{
    [Fact]
    public void AddApiIntegration_BindsBaseUrlFromConfiguration()
    {
        var configuration = BuildConfiguration("https://api.goodhamburguer.local");
        var services = new ServiceCollection();

        services.AddApiIntegration(configuration);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ApiOptions>>().Value;

        options.BaseUrl.Should().Be("https://api.goodhamburguer.local");
    }

    private static IConfiguration BuildConfiguration(string? baseUrl)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Api:BaseUrl"] = baseUrl
            })
            .Build();
    }
}
