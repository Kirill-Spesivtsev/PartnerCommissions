using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Application.Abstractions;

public interface ICommissionRepository
{
    Task<IReadOnlyList<Commission>> GetUnpaidAsync(int limit, CancellationToken cancellationToken);
    
    Task MarkPaidAsync(IReadOnlyCollection<Guid> commissionIds, DateTimeOffset paidAt, CancellationToken cancellationToken);
}
