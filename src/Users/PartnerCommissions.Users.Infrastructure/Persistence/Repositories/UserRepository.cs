using Microsoft.EntityFrameworkCore;
using Npgsql;
using PartnerCommissions.Users.Application.Abstractions;
using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UsersDbContext db) : IUserRepository
{
    public Task<UserDetails?> GetDetailsAsync(string externalId, CancellationToken cancellationToken)
    {
        return db.Users
            .AsNoTracking()
            .Where(u => u.ExternalId == externalId)
            .Select(u => new UserDetails(
                u.ExternalId,
                u.Referrer != null ? u.Referrer.ExternalId : null,
                u.CreatedAt,
                u.ReferrerSetAt))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> TryAddAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<string>> GetAncestorsAsync(string externalId, CancellationToken cancellationToken)
    {
        return await db.Database
            .SqlQuery<string>(
            $"""
                WITH RECURSIVE ancestors AS (
                    SELECT r."ExternalId", r."ReferrerId", 1 AS "Level"
                    FROM "Users" u
                    JOIN "Users" r ON r."Id" = u."ReferrerId"
                    WHERE u."ExternalId" = {externalId}

                    UNION ALL

                    SELECT r."ExternalId", r."ReferrerId", a."Level" + 1
                    FROM ancestors a
                    JOIN "Users" r ON r."Id" = a."ReferrerId"
                    WHERE a."Level" < 9
                )
                SELECT "ExternalId" AS "Value"
                FROM ancestors
                ORDER BY "Level"
            """)
            .ToListAsync(cancellationToken);
    }
}
