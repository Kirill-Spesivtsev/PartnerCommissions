namespace PartnerCommissions.Wallets.Infrastructure.BackgroundServices;

public class PayoutOptions
{
    public const string SectionName = "Payout";

    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(30);
    public int BatchSize { get; set; } = 100;
}
