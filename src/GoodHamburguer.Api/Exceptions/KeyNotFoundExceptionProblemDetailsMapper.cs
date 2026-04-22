using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Exceptions;

public sealed class KeyNotFoundExceptionProblemDetailsMapper : ExceptionProblemDetailsMapper<KeyNotFoundException>
{
    protected override ProblemDetails Map(KeyNotFoundException exception, HttpContext httpContext)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Resource not found.",
            Detail = exception.Message,
            Type = "https://httpstatuses.com/404",
            Instance = httpContext.Request.Path
        };
    }
}
