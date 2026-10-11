using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PartnerCommissions.Wallets.Application.Models;
using PartnerCommissions.Wallets.Application.Services;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Api.Controllers;

[ApiController]
[Route("api/v1/wallets")]
[Produces("application/json")]
public class WalletsController(WalletService walletService) : ControllerBase
{
    [HttpGet("{userExternalId}")]
    public async Task<ActionResult<WalletBalance>> GetBalance(string userExternalId, CancellationToken cancellationToken)
    {
        return await walletService.GetBalanceAsync(userExternalId, cancellationToken);
    }

    [HttpGet("{userExternalId}/transactions")]
    public async Task<ActionResult<IReadOnlyList<WalletTransaction>>> ListTransactions(
        string userExternalId,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 100)] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        return Ok(await walletService.ListTransactionsAsync(userExternalId, page, pageSize, cancellationToken));
    }
}
