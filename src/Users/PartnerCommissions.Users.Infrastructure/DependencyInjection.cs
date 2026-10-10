using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Users.Infrastructure.Persistence;

namespace PartnerCommissions.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDb");

        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
