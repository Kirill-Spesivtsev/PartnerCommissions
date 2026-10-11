using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Wallets.Application.Abstractions;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Infrastructure.Persistence.Repositories;

public class WalletTransactionRepository(WalletsDbContext db) : IWalletTransactionRepository
{
    public async Task<IReadOnlySet<Guid>> GetExistingCommissionIdsAsync(IReadOnlyCollection<Guid> commissionIds, CancellationToken cancellationToken)
    {
        var existing = await db.WalletTransactions
            .Where(t => commissionIds.Contains(t.CommissionId))
            .Select(t => t.CommissionId)
            .ToListAsync(cancellationToken);

        return existing.ToHashSet();
    }

    public async Task AddRangeAsync(IReadOnlyCollection<WalletTransaction> transactions, CancellationToken cancellationToken)
    {
        db.WalletTransactions.AddRange(transactions);
        await db.SaveChangesAsync(cancellationToken);
    }
}
