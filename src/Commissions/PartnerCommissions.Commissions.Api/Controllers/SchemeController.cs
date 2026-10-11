using Microsoft.AspNetCore.Mvc;
using PartnerCommissions.Commissions.Api.Models;
using PartnerCommissions.Commissions.Application.Models;
using PartnerCommissions.Commissions.Application.Services;

namespace PartnerCommissions.Commissions.Api.Controllers;

[ApiController]
[Route("api/v1/admin/scheme")]
[Produces("application/json")]
public class SchemeController(SchemeService schemeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SchemeDetails>> Get(CancellationToken cancellationToken)
    {
        return await schemeService.GetCurrentAsync(cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> Set(SetSchemeRequest request, CancellationToken cancellationToken)
    {
        await schemeService.SetAsync(request.Scheme, cancellationToken);

        return NoContent();
    }
}
