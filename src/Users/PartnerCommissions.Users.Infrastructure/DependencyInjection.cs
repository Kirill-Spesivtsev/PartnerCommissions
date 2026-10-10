using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Users.Application.Abstractions;
using PartnerCommissions.Users.Infrastructure.Persistence;
using PartnerCommissions.Users.Infrastructure.Persistence.Repositories;

namespace PartnerCommissions.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UsersDb");

        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
