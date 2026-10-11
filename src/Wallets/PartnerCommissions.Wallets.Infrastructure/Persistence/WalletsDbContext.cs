using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Wallets.Domain.Entities;

namespace PartnerCommissions.Wallets.Infrastructure.Persistence;

public class WalletsDbContext(DbContextOptions<WalletsDbContext> options) : DbContext(options)
{
    public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletsDbContext).Assembly);
    }
}
