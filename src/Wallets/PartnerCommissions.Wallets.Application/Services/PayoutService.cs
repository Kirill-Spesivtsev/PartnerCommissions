using PartnerCommissions.Wallets.Application.Abstractions;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Application.Services;

public class PayoutService(ICommissionsClient commissionsClient, IWalletTransactionRepository transactions)
{
    public async Task<int> PayAsync(int batchSize, CancellationToken cancellationToken)
    {
        var unpaid = await commissionsClient.GetUnpaidAsync(batchSize, cancellationToken);

        if (unpaid.Count == 0)
            return 0;

        var commissionIds = unpaid.Select(c => c.CommissionId).ToList();
        var alreadyPaid = await transactions.GetExistingCommissionIdsAsync(commissionIds, cancellationToken);
        var paidAt = DateTimeOffset.UtcNow;

        var newTransactions = unpaid
            .Where(c => !alreadyPaid.Contains(c.CommissionId))
            .Select(c => new WalletTransaction
            {
                CommissionId = c.CommissionId,
                UserExternalId = c.PartnerExternalId,
                EventExternalId = c.EventExternalId,
                Level = c.Level,
                Amount = c.Amount,
                AccruedAt = c.AccruedAt,
                PaidAt = paidAt
            })
            .ToList();

        if (newTransactions.Count > 0)
            await transactions.AddRangeAsync(newTransactions, cancellationToken);

        await commissionsClient.MarkPaidAsync(commissionIds, paidAt, cancellationToken);

        return newTransactions.Count;
    }
}
