using PartnerCommissions.Wallets.Application.Models;

namespace PartnerCommissions.Wallets.Application.Abstractions;

public interface ICommissionsClient
{
    Task<IReadOnlyList<UnpaidCommission>> GetUnpaidAsync(int limit, CancellationToken cancellationToken);
    Task MarkPaidAsync(IReadOnlyCollection<Guid> commissionIds, DateTimeOffset paidAt, CancellationToken cancellationToken);
}
