using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Application.Abstractions;

public interface IWalletTransactionRepository
{
    Task<IReadOnlySet<Guid>> GetExistingCommissionIdsAsync(IReadOnlyCollection<Guid> commissionIds, CancellationToken cancellationToken);
    Task AddRangeAsync(IReadOnlyCollection<WalletTransaction> transactions, CancellationToken cancellationToken);
    Task<decimal> GetBalanceAsync(string userExternalId, CancellationToken cancellationToken);
    Task<IReadOnlyList<WalletTransaction>> ListByUserAsync(string userExternalId, int page, int pageSize, CancellationToken cancellationToken);
}
