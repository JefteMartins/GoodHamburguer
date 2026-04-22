using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodHamburguer.Api.Exceptions;

public sealed class GlobalExceptionFilter(
    IEnumerable<IExceptionProblemDetailsMapper> mappers,
    ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    private readonly IReadOnlyList<IExceptionProblemDetailsMapper> _mappers = mappers.ToArray();

    public void OnException(ExceptionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var mapper = ResolveMapper(context.Exception);
        var problemDetails = mapper?.Map(context.Exception, context.HttpContext) ?? CreateUnhandledProblemDetails(context);

        if (mapper is null)
        {
            logger.LogError(context.Exception, "Unhandled exception while processing {Path}.", context.HttpContext.Request.Path);
        }
        else
        {
            logger.LogWarning(context.Exception, "Mapped exception {ExceptionType} for {Path}.", context.Exception.GetType().Name, context.HttpContext.Request.Path);
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
        context.ExceptionHandled = true;
    }

    private IExceptionProblemDetailsMapper? ResolveMapper(Exception exception)
    {
        return _mappers
            .Where(mapper => mapper.ExceptionType.IsAssignableFrom(exception.GetType()))
            .OrderBy(mapper => GetTypeDistance(exception.GetType(), mapper.ExceptionType))
            .FirstOrDefault();
    }

    private static int GetTypeDistance(Type concreteType, Type candidateBaseType)
    {
        var distance = 0;
        var current = concreteType;

        while (current is not null)
        {
            if (current == candidateBaseType)
            {
                return distance;
            }

            current = current.BaseType;
            distance++;
        }

        return int.MaxValue;
    }

    private static ProblemDetails CreateUnhandledProblemDetails(ExceptionContext context)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "The server failed to process the request.",
            Type = "https://httpstatuses.com/500",
            Instance = context.HttpContext.Request.Path
        };
    }
}
