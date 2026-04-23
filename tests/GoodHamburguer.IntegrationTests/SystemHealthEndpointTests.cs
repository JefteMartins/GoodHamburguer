using FluentAssertions;
using GoodHamburguer.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace GoodHamburguer.IntegrationTests;

public sealed class SystemHealthEndpointTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SystemHealthEndpointTests(MySqlWebApplicationFactory factory)
    {
        _client = factory.CreateApiClient();
    }

    [Fact]
    public async Task Liveness_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health/live");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Readiness_ShouldReturnOk_WhenMySqlIsAvailable()
    {
        var response = await _client.GetAsync("/health/ready");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SystemInfo_ShouldReturnServiceMetadata()
    {
        var response = await _client.GetAsync("/api/v1/system/info");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<SystemInfoContract>();
        content.Should().NotBeNull();
        content!.Service.Should().Be("GoodHamburguer.Api");
        content.Version.Should().Be("v1");
        content.Status.Should().Be("phase-12-ready");
    }

    [Fact]
    public async Task Readiness_ShouldReturnStructuredPayload()
    {
        var response = await _client.GetAsync("/health/ready");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<HealthResponseContract>();
        payload.Should().NotBeNull();
        payload!.Status.Should().Be("Healthy");
        payload.Entries.Should().ContainKey("mysql");
    }

    public sealed class SystemInfoContract
    {
        public string? Service { get; init; }

        public string? Version { get; init; }

        public string? Status { get; init; }
    }

    public sealed class HealthResponseContract
    {
        public string? Status { get; init; }

        public required IDictionary<string, HealthEntryContract> Entries { get; init; }
    }

    public sealed class HealthEntryContract
    {
        public string? Status { get; init; }
    }
}
