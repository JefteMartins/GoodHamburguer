using FluentAssertions;
using GoodHamburguer.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace GoodHamburguer.IntegrationTests;

public sealed class OperationalLogsEndpointTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public OperationalLogsEndpointTests(MySqlWebApplicationFactory factory)
    {
        _client = factory.CreateApiClient();
    }

    [Fact]
    public async Task List_ShouldReturnApplicationLogsFilteredByTypeAndPeriod()
    {
        var from = DateTimeOffset.UtcNow.AddMinutes(-2).ToString("O");
        var to = DateTimeOffset.UtcNow.AddMinutes(2).ToString("O");

        var createResponse = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-x-burger",
            sideItemCode = "side-fries"
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var response = await _client.GetAsync($"/api/v1/operational-logs?type=application&from={Uri.EscapeDataString(from)}&to={Uri.EscapeDataString(to)}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var logs = await response.Content.ReadFromJsonAsync<List<OperationalLogContract>>();

        logs.Should().NotBeNull();
        logs!
            .Should()
            .Contain(log =>
                log.Type == "application"
                && log.Route == "/api/v1/orders"
                && log.Method == "POST"
                && log.Payload.Contains("sandwichItemCode", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task List_ShouldReturnErrorLogsWithExceptionContext()
    {
        var from = DateTimeOffset.UtcNow.AddMinutes(-2).ToString("O");
        var to = DateTimeOffset.UtcNow.AddMinutes(2).ToString("O");

        var errorResponse = await _client.GetAsync("/api/v1/system/test/unknown-exception");
        errorResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var response = await _client.GetAsync($"/api/v1/operational-logs?type=error&from={Uri.EscapeDataString(from)}&to={Uri.EscapeDataString(to)}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var logs = await response.Content.ReadFromJsonAsync<List<OperationalLogContract>>();

        logs.Should().NotBeNull();
        logs!
            .Should()
            .Contain(log =>
                log.Type == "error"
                && log.Route == "/api/v1/system/test/unknown-exception"
                && log.ExceptionType == nameof(InvalidOperationException)
                && !string.IsNullOrWhiteSpace(log.ErrorMessage));
    }

    public sealed class OperationalLogContract
    {
        public required string Type { get; init; }

        public required string Route { get; init; }

        public required string Method { get; init; }

        public string Payload { get; init; } = string.Empty;

        public string? ExceptionType { get; init; }

        public string? ErrorMessage { get; init; }
    }
}
