using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PartnerCommissions.Commissions.Api.Models;
using PartnerCommissions.Commissions.Application.Models;
using PartnerCommissions.Commissions.Application.Services;

namespace PartnerCommissions.Commissions.Api.Controllers;

[ApiController]
[Route("api/v1/commissions")]
[Produces("application/json")]
public class CommissionsController(CommissionService commissionService) : ControllerBase
{
    [HttpGet("unpaid")]
    public async Task<ActionResult<IReadOnlyList<UnpaidCommission>>> GetUnpaid([FromQuery, Range(1, 1000)] int limit = 100, CancellationToken cancellationToken = default)
    {
        return Ok(await commissionService.GetUnpaidAsync(limit, cancellationToken));
    }

    [HttpPost("mark-paid")]
    public async Task<IActionResult> MarkPaid(MarkPaidRequest request, CancellationToken cancellationToken)
    {
        await commissionService.MarkPaidAsync(request.CommissionIds, request.PaidAt, cancellationToken);

        return NoContent();
    }
}
