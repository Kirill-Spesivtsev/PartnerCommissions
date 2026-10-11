using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Wallets.Infrastructure.Persistence;

namespace PartnerCommissions.Wallets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WalletsDb");

        services.AddDbContext<WalletsDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
