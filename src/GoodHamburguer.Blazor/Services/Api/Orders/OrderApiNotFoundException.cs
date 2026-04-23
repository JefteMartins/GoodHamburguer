namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class OrderApiNotFoundException(string message) : Exception(message);
