using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Commissions.Infrastructure.Persistence;

namespace PartnerCommissions.Commissions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CommissionsDb");

        services.AddDbContext<CommissionsDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
