using GoodHamburguer.Blazor.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string MissingBaseUrlMessage = "Api:BaseUrl must be configured.";
    private const string InvalidBaseUrlMessage = "Api:BaseUrl must be a valid absolute URI.";

    public static IServiceCollection AddApiIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ApiOptions>()
            .Bind(configuration.GetSection(ApiOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.BaseUrl),
                MissingBaseUrlMessage)
            .Validate(
                options => TryBuildVersionedApiBaseAddress(options.BaseUrl, out _),
                InvalidBaseUrlMessage)
            .ValidateOnStart();

        services.AddHttpClient(GoodHamburguer.Blazor.Program.ApiHttpClientName, (serviceProvider, client) =>
        {
            ApiOptions apiOptions;

            try
            {
                apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>().Value;
            }
            catch (OptionsValidationException exception)
            {
                throw new InvalidOperationException(exception.Message, exception);
            }

            if (!TryBuildVersionedApiBaseAddress(apiOptions.BaseUrl, out var baseAddress))
            {
                throw new InvalidOperationException(
                    string.IsNullOrWhiteSpace(apiOptions.BaseUrl) ? MissingBaseUrlMessage : InvalidBaseUrlMessage);
            }

            client.BaseAddress = baseAddress;
        });

        return services;
    }

    private static bool TryBuildVersionedApiBaseAddress(string? baseUrl, out Uri? versionedBaseAddress)
    {
        versionedBaseAddress = null;

        if (string.IsNullOrWhiteSpace(baseUrl) ||
            !Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri) ||
            !string.IsNullOrEmpty(baseUri.Query) ||
            !string.IsNullOrEmpty(baseUri.Fragment))
        {
            return false;
        }

        var builder = new UriBuilder(baseUri);
        var normalizedPath = builder.Path.TrimEnd('/');
        builder.Path = $"{normalizedPath}/api/v1/";
        builder.Query = string.Empty;
        builder.Fragment = string.Empty;
        versionedBaseAddress = builder.Uri;
        return true;
    }
}
