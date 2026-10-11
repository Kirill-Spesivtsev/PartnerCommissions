using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Application.Models;

public record CommissionDetails(
    string PartnerExternalId,
    int Level,
    decimal Amount,
    SchemaType SchemaType,
    DateTimeOffset AccruedAt,
    DateTimeOffset? PaidAt);
