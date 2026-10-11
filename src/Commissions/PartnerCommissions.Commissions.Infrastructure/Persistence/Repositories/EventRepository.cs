using Microsoft.EntityFrameworkCore;
using Npgsql;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Application.Exceptions;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence.Repositories;

public class EventRepository(CommissionsDbContext db) : IEventRepository
{
    public async Task<ProfitEvent> GetByIdAsync(string externalId, CancellationToken cancellationToken)
    {
        var profitEvent = await db.ProfitEvents
            .AsNoTracking()
            .Include(e => e.Commissions)
            .SingleOrDefaultAsync(e => e.ExternalId == externalId, cancellationToken);

        if (profitEvent is null)
            throw new NotFoundException($"Event '{externalId}' was not found.");

        return profitEvent;
    }

    public async Task<bool> TryAddAsync(ProfitEvent profitEvent, CancellationToken cancellationToken)
    {
        db.ProfitEvents.Add(profitEvent);

        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<ProfitEvent>> ListByUserAsync(string userExternalId, int page, int pageSize, CancellationToken cancellationToken)
    {
        return await db.ProfitEvents
            .AsNoTracking()
            .Where(e => e.UserExternalId == userExternalId)
            .OrderByDescending(e => e.ReceivedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
