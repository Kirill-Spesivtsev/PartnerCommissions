using Microsoft.AspNetCore.Mvc;
using PartnerCommissions.Users.Api.Models;
using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Application.Services;

namespace PartnerCommissions.Users.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserDetails>> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userService.CreateAsync(request.ExternalId, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { externalId = user.ExternalId }, user);
    }

    [HttpGet("{externalId}")]
    public async Task<ActionResult<UserDetails>> GetById(string externalId, CancellationToken cancellationToken)
    {
        return await userService.GetByIdAsync(externalId, cancellationToken);
    }
}
