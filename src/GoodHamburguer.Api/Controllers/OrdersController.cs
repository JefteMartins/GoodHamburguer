using Asp.Versioning;
using FluentValidation;
using GoodHamburguer.Application.Orders.Contracts;
using GoodHamburguer.Application.Orders.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
public sealed class OrdersController(IOrderAppService orderAppService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<OrderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<OrderResponse>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderAppService.CreateAsync(request, cancellationToken);
            return Ok(order);
        }
        catch (ValidationException exception)
        {
            return UnprocessableEntity(ToValidationProblemDetails(exception));
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<OrderResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> Update(
        Guid id,
        [FromBody] UpdateOrderRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderAppService.UpdateAsync(id, request, cancellationToken);
            return Ok(order);
        }
        catch (ValidationException exception)
        {
            return UnprocessableEntity(ToValidationProblemDetails(exception));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    private static ValidationProblemDetails ToValidationProblemDetails(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).Distinct().ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "One or more validation errors occurred."
        };
    }
}
