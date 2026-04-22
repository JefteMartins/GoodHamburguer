using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Exceptions;

public sealed class FluentValidationExceptionProblemDetailsMapper : ExceptionProblemDetailsMapper<ValidationException>
{
    protected override ProblemDetails Map(ValidationException exception, HttpContext httpContext)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).Distinct().ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "One or more validation errors occurred.",
            Detail = "The request payload contains invalid values.",
            Type = "https://httpstatuses.com/422",
            Instance = httpContext.Request.Path
        };
    }
}
