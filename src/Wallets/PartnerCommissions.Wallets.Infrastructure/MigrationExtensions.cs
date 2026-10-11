using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Wallets.Infrastructure.Persistence;

namespace PartnerCommissions.Wallets.Infrastructure;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WalletsDbContext>();

        await db.Database.MigrateAsync(cancellationToken);
    }
}
