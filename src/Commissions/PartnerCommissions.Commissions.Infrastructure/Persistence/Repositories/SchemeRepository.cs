using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Repositories;

public class SchemeRepository(CommissionsDbContext db) : ISchemeRepository
{
    public Task<CommissionSettings> GetCurrentAsync(CancellationToken cancellationToken)
    {
        return db.CommissionSettings
            .AsNoTracking()
            .SingleAsync(s => s.Id == 1, cancellationToken);
    }
}
