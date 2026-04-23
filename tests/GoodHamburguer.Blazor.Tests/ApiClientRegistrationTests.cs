using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburguer.Blazor.Tests;

public class ApiClientRegistrationTests
{
    [Fact]
    public void AddApiIntegration_ComposesApiV1BasePath()
    {
        var configuration = BuildConfiguration("https://api.goodhamburguer.local/base");
        var services = new ServiceCollection();
        services.AddApiIntegration(configuration);

        using var provider = services.BuildServiceProvider();
        var clientFactory = provider.GetRequiredService<IHttpClientFactory>();

        var client = clientFactory.CreateClient(Program.ApiHttpClientName);

        client.BaseAddress.Should().Be(new Uri("https://api.goodhamburguer.local/base/api/v1/"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-valid-uri")]
    public void AddApiIntegration_ThrowsWhenBaseUrlIsInvalid(string invalidBaseUrl)
    {
        var configuration = BuildConfiguration(invalidBaseUrl);
        var services = new ServiceCollection();
        services.AddApiIntegration(configuration);

        using var provider = services.BuildServiceProvider();
        var clientFactory = provider.GetRequiredService<IHttpClientFactory>();

        var createClient = () => clientFactory.CreateClient(Program.ApiHttpClientName);

        createClient.Should().Throw<InvalidOperationException>();
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
