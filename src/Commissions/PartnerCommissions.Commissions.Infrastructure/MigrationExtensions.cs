using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Commissions.Infrastructure.Persistence;

namespace PartnerCommissions.Commissions.Infrastructure;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CommissionsDbContext>();

        await db.Database.MigrateAsync(cancellationToken);
    }
}
