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
            status = "foundation-ready"
        });
    }
}
