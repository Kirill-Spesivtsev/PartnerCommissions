using System.Data;
using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Users.Application.Abstractions;

namespace PartnerCommissions.Users.Infrastructure.Persistence;

public class UnitOfWork(UsersDbContext db) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
    }
}
