using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Domain.Entities;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Repositories;

public class SchemeRepository(CommissionsDbContext db) : ISchemeRepository
{
    public Task<CommissionSettings> GetCurrentAsync(CancellationToken cancellationToken)
    {
        return db.CommissionSettings
            .AsNoTracking()
            .SingleAsync(s => s.Id == 1, cancellationToken);
    }

    public Task SetAsync(SchemaType scheme, DateTimeOffset updatedAt, CancellationToken cancellationToken)
    {
        return db.CommissionSettings
            .Where(s => s.Id == 1)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.CurrentScheme, scheme)
                .SetProperty(s => s.UpdatedAt, updatedAt), cancellationToken);
    }
}
