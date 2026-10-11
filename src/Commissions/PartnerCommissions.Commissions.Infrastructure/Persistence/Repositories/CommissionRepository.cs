using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Repositories;

public class CommissionRepository(CommissionsDbContext db) : ICommissionRepository
{
    public async Task<IReadOnlyList<Commission>> GetUnpaidAsync(int limit, CancellationToken cancellationToken)
    {
        return await db.Commissions
            .AsNoTracking()
            .Include(c => c.Event)
            .Where(c => c.PaidAt == null)
            .OrderBy(c => c.AccruedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public Task MarkPaidAsync(IReadOnlyCollection<Guid> commissionIds, DateTimeOffset paidAt, CancellationToken cancellationToken)
    {
        return db.Commissions
            .Where(c => commissionIds.Contains(c.Id) && c.PaidAt == null)
            .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.PaidAt, paidAt), cancellationToken);
    }
}
