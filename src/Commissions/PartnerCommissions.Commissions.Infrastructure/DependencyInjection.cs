using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Infrastructure.Clients;
using PartnerCommissions.Commissions.Infrastructure.Persistence;
using PartnerCommissions.Commissions.Infrastructure.Persistence.Repositories;

namespace PartnerCommissions.Commissions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CommissionsDb");

        services.AddDbContext<CommissionsDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ISchemeRepository, SchemeRepository>();
        services.AddScoped<ICommissionRepository, CommissionRepository>();

        services.AddHealthChecks().AddDbContextCheck<CommissionsDbContext>(tags: ["ready"]);

        services.Configure<UsersApiOptions>(configuration.GetSection(UsersApiOptions.SectionName));

        services.AddHttpClient<IUsersClient, UsersClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<UsersApiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromMilliseconds(500);
                options.CircuitBreaker.MinimumThroughput = 20;
                options.CircuitBreaker.FailureRatio = 0.5;
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
            });

        return services;
    }
}
