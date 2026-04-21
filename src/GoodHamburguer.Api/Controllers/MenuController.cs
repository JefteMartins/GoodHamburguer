using Asp.Versioning;
using GoodHamburguer.Application.Menu.Contracts;
using GoodHamburguer.Application.Menu.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/menu")]
public sealed class MenuController(IMenuAppService menuAppService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<MenuCategoryResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MenuCategoryResponse>>> GetMenu(CancellationToken cancellationToken)
    {
        var menu = await menuAppService.GetMenuAsync(cancellationToken);
        return Ok(menu);
    }
}
