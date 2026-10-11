using Microsoft.AspNetCore.Mvc;
using PartnerCommissions.Commissions.Api.Models;
using PartnerCommissions.Commissions.Application.Models;
using PartnerCommissions.Commissions.Application.Services;

namespace PartnerCommissions.Commissions.Api.Controllers;

[ApiController]
[Route("api/v1/events")]
[Produces("application/json")]
public class EventsController(EventService eventService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EventDetails>> Create(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var (profitEvent, created) = await eventService.CreateAsync(request.ExternalId, request.UserExternalId, request.Profit, cancellationToken);

        if (!created)
            return Ok(profitEvent);

        return CreatedAtAction(nameof(GetById), new { externalId = profitEvent.ExternalId }, profitEvent);
    }

    [HttpGet("{externalId}")]
    public async Task<ActionResult<EventDetails>> GetById(string externalId, CancellationToken cancellationToken)
    {
        return await eventService.GetByIdAsync(externalId, cancellationToken);
    }
}
