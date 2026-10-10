using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Infrastructure.Persistence;

public class CommissionsDbContext(DbContextOptions<CommissionsDbContext> options) : DbContext(options)
{
    public DbSet<ProfitEvent> ProfitEvents => Set<ProfitEvent>();
    public DbSet<Commission> Commissions => Set<Commission>();
    public DbSet<CommissionSettings> CommissionSettings => Set<CommissionSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommissionsDbContext).Assembly);
    }
}
