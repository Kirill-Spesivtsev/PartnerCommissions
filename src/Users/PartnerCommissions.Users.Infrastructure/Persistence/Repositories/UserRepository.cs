using Microsoft.EntityFrameworkCore;
using Npgsql;
using PartnerCommissions.Users.Application.Abstractions;
using PartnerCommissions.Users.Application.Exceptions;
using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Infrastructure.Persistence.Repositories;

public class UserRepository(UsersDbContext db) : IUserRepository
{
    public async Task<User> GetByIdAsync(string externalId, CancellationToken cancellationToken)
    {
        var user = await db.Users
            .Include(u => u.Referrer)
            .SingleOrDefaultAsync(u => u.ExternalId == externalId, cancellationToken);

        if (user is null)
            throw new NotFoundException($"User '{externalId}' was not found.");

        return user;
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
        var chain = await db.Database
            .SqlQuery<string>(
            $"""
                WITH RECURSIVE ancestors AS (
                    SELECT u."ExternalId", u."ReferrerId", 0 AS "Level"
                    FROM "Users" u
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

        if (chain.Count == 0)
            throw new NotFoundException($"User '{externalId}' was not found.");

        return chain.Skip(1).ToList();
    }

    public async Task<IReadOnlyList<ReferralTreeNode>> GetDescendantsAsync(string externalId, int maxDepth, CancellationToken cancellationToken)
    {
        var rows = await db.Database
            .SqlQuery<DescendantRow>(
            $"""
                WITH RECURSIVE descendants AS (
                    SELECT u."Id", u."ReferrerId", u."ExternalId", 0 AS "Depth"
                    FROM "Users" u
                    WHERE u."ExternalId" = {externalId}

                    UNION ALL

                    SELECT r."Id", r."ReferrerId", r."ExternalId", d."Depth" + 1
                    FROM descendants d
                    JOIN "Users" r ON r."ReferrerId" = d."Id"
                    WHERE d."Depth" < {maxDepth}
                )
                SELECT "Id", "ReferrerId", "ExternalId", "Depth"
                FROM descendants
            """)
            .ToListAsync(cancellationToken);

        var root = rows.SingleOrDefault(r => r.Depth == 0);

        if (root is null)
            throw new NotFoundException($"User '{externalId}' was not found.");

        var children = rows.ToLookup(r => (r.ReferrerId, r.Depth));

        ReferralTreeNode Build(DescendantRow row) =>
            new(row.ExternalId, children[(row.Id, row.Depth + 1)].Select(Build).ToList());

        return children[(root.Id, 1)].Select(Build).ToList();
    }

    private class DescendantRow
    {
        public Guid Id { get; init; }
        public Guid? ReferrerId { get; init; }
        public required string ExternalId { get; init; }
        public int Depth { get; init; }
    }
}
