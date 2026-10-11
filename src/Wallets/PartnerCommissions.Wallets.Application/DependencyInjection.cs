using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Wallets.Application.Services;

namespace PartnerCommissions.Wallets.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PayoutService>();

        return services;
    }
}
