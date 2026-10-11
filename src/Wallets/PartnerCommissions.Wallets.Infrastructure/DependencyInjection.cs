using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PartnerCommissions.Wallets.Application.Abstractions;
using PartnerCommissions.Wallets.Infrastructure.Clients;
using PartnerCommissions.Wallets.Infrastructure.BackgroundServices;
using PartnerCommissions.Wallets.Infrastructure.Persistence;
using PartnerCommissions.Wallets.Infrastructure.Persistence.Repositories;

namespace PartnerCommissions.Wallets.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WalletsDb");

        services.AddDbContext<WalletsDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();

        services.Configure<PayoutOptions>(configuration.GetSection(PayoutOptions.SectionName));
        services.AddHostedService<PayoutWorker>();

        services.Configure<CommissionsApiOptions>(configuration.GetSection(CommissionsApiOptions.SectionName));

        services.AddHttpClient<ICommissionsClient, CommissionsClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CommissionsApiOptions>>().Value;
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
