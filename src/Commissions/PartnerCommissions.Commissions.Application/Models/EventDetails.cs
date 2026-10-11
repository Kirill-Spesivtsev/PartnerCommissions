namespace PartnerCommissions.Commissions.Application.Models;

public record EventDetails(
    string ExternalId,
    string UserExternalId,
    decimal Profit,
    DateTimeOffset ReceivedAt,
    IReadOnlyList<CommissionDetails> Commissions);
