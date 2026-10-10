using Microsoft.EntityFrameworkCore;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Infrastructure.Persistence;

public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }
}
