using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Exceptions;

public interface IExceptionProblemDetailsMapper
{
    Type ExceptionType { get; }

    ProblemDetails Map(Exception exception, HttpContext httpContext);
}
