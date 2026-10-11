namespace PartnerCommissions.Commissions.Application.Models;

public record EventSummary(
    string ExternalId,
    string UserExternalId,
    decimal Profit,
    DateTimeOffset ReceivedAt);
