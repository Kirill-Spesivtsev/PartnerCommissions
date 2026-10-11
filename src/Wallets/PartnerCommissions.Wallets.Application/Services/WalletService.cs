using PartnerCommissions.Wallets.Application.Abstractions;
using PartnerCommissions.Wallets.Application.Models;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Application.Services;

public class WalletService(IWalletTransactionRepository transactions)
{
    public async Task<WalletBalance> GetBalanceAsync(string userExternalId, CancellationToken cancellationToken)
    {
        var balance = await transactions.GetBalanceAsync(userExternalId, cancellationToken);

        return new WalletBalance(userExternalId, balance);
    }

    public Task<IReadOnlyList<WalletTransaction>> ListTransactionsAsync(string userExternalId, int page, int pageSize, CancellationToken cancellationToken)
    {
        return transactions.ListByUserAsync(userExternalId, page, pageSize, cancellationToken);
    }
}
