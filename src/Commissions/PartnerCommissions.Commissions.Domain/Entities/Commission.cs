using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Domain.Entities;

public class Commission
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public ProfitEvent Event { get; set; } = null!;

    public required string PartnerExternalId { get; set; }
    public int Level { get; set; }
    public SchemaType SchemaType { get; set; }
    public decimal Amount { get; set; }

    public DateTimeOffset AccruedAt { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
}
