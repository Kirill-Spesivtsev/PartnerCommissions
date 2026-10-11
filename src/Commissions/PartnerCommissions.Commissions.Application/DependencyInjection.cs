using Microsoft.Extensions.DependencyInjection;
using PartnerCommissions.Commissions.Application.Services;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<CommissionScheme, LinearScheme>();
        services.AddSingleton<CommissionScheme, FibonacciScheme>();

        services.AddScoped<EventService>();

        return services;
    }
}
