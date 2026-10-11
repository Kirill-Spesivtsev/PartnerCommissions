namespace PartnerCommissions.Wallets.Domain.Entities;

public class WalletTransaction
{
    public Guid CommissionId { get; set; }
    public required string UserExternalId { get; set; }
    public required string EventExternalId { get; set; }
    public int Level { get; set; }
    public decimal Amount { get; set; }

    public DateTimeOffset AccruedAt { get; set; }
    public DateTimeOffset PaidAt { get; set; }
}
