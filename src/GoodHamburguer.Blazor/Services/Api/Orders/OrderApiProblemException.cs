namespace GoodHamburguer.Blazor.Services.Api.Orders;

public sealed class OrderApiProblemException(string message) : Exception(message);
