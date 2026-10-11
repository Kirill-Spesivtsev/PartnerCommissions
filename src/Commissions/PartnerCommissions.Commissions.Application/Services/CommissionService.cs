using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Application.Models;

namespace PartnerCommissions.Commissions.Application.Services;

public class CommissionService(ICommissionRepository commissions)
{
    public async Task<IReadOnlyList<UnpaidCommission>> GetUnpaidAsync(int limit, CancellationToken cancellationToken)
    {
        var unpaid = await commissions.GetUnpaidAsync(limit, cancellationToken);

        return unpaid
            .Select(c => new UnpaidCommission(c.Id, c.PartnerExternalId, c.Event.ExternalId, c.Level, c.Amount, c.AccruedAt))
            .ToList();
    }

    public Task MarkPaidAsync(IReadOnlyCollection<Guid> commissionIds, DateTimeOffset paidAt, CancellationToken cancellationToken)
    {
        return commissions.MarkPaidAsync(commissionIds, paidAt, cancellationToken);
    }
}
