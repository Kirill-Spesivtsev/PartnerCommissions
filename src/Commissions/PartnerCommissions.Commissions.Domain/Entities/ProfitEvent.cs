namespace PartnerCommissions.Commissions.Domain.Entities;

public class ProfitEvent
{
    public Guid Id { get; set; }
    public required string ExternalId { get; set; }
    public required string UserExternalId { get; set; }
    public decimal Profit { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }

    public List<Commission> Commissions { get; set; } = [];
}
