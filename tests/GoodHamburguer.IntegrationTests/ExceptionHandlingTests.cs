using FluentAssertions;
using GoodHamburguer.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace GoodHamburguer.IntegrationTests;

public sealed class ExceptionHandlingTests : IClassFixture<MySqlWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ExceptionHandlingTests(MySqlWebApplicationFactory factory)
    {
        _client = factory.CreateApiClient();
    }

    [Fact]
    public async Task KnownException_ShouldReturnProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/system/test/known-exception");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();

        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.NotFound);
        problem.Title.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UnknownException_ShouldReturnInternalServerErrorProblemDetails()
    {
        var response = await _client.GetAsync("/api/v1/system/test/unknown-exception");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();

        problem.Should().NotBeNull();
        problem!.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        problem.Title.Should().NotBeNullOrWhiteSpace();
    }

    public sealed class ProblemDetailsContract
    {
        public string? Type { get; init; }

        public string? Title { get; init; }

        public int? Status { get; init; }

        public string? Detail { get; init; }
    }
}
