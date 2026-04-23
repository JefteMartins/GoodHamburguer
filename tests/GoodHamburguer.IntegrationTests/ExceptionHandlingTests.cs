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

    [Fact]
    public async Task ValidationException_ShouldReturnValidationProblemDetails_WithInstanceAndErrors()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/orders", new
        {
            sandwichItemCode = "sandwich-does-not-exist"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemContract>();

        problem.Should().NotBeNull();
        problem!.Type.Should().Be("https://httpstatuses.com/422");
        problem.Title.Should().Be("One or more validation errors occurred.");
        problem.Instance.Should().Be("/api/v1/orders");
        problem.Errors.Should().ContainKey("sandwichItemCode");
    }

    public class ProblemDetailsContract
    {
        public string? Type { get; init; }

        public string? Title { get; init; }

        public int? Status { get; init; }

        public string? Detail { get; init; }
    }

    public sealed class ValidationProblemContract : ProblemDetailsContract
    {
        public required IDictionary<string, string[]> Errors { get; init; }

        public string? Instance { get; init; }
    }
}
