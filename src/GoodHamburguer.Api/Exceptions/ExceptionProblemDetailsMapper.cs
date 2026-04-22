using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Exceptions;

public abstract class ExceptionProblemDetailsMapper<TException> : IExceptionProblemDetailsMapper
    where TException : Exception
{
    public Type ExceptionType => typeof(TException);

    public ProblemDetails Map(Exception exception, HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(httpContext);

        return Map((TException)exception, httpContext);
    }

    protected abstract ProblemDetails Map(TException exception, HttpContext httpContext);
}
