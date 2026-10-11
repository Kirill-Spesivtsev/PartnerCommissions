using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PartnerCommissions.Wallets.Application.Services;

namespace PartnerCommissions.Wallets.Infrastructure.BackgroundServices;

public class PayoutWorker(IServiceScopeFactory scopeFactory, IOptions<PayoutOptions> options, ILogger<PayoutWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(options.Value.Interval);

        try
        {
            do
            {
                await PayAsync(stoppingToken);
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }

    private async Task PayAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var payoutService = scope.ServiceProvider.GetRequiredService<PayoutService>();

            var paid = await payoutService.PayAsync(options.Value.BatchSize, stoppingToken);

            if (paid > 0)
                logger.LogInformation("Paid {Count} commissions", paid);
        }
        catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
        {
            logger.LogError(ex, "Payout cycle failed");
        }
    }
}
