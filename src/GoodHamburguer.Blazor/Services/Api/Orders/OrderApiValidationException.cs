namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class OrderApiValidationException(
    IReadOnlyDictionary<string, string[]> errors,
    string message)
    : Exception(message)
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
