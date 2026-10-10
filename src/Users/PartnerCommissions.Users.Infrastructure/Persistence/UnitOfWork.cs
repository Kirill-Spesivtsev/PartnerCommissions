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

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        await db.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await db.SaveChangesAsync(cancellationToken);
        await db.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await db.Database.RollbackTransactionAsync(cancellationToken);
    }
}
