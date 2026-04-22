using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            service = "GoodHamburguer.Api",
            version = "v1",
            status = "phase-8-ready"
        });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("test/known-exception")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ThrowKnownException()
    {
        throw new KeyNotFoundException("Integration test known exception.");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("test/unknown-exception")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public IActionResult ThrowUnknownException()
    {
        throw new InvalidOperationException("Integration test unknown exception.");
    }
}
