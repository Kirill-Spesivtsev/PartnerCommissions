namespace PartnerCommissions.Wallets.Application.Models;

public record UnpaidCommission(
    Guid CommissionId,
    string PartnerExternalId,
    string EventExternalId,
    int Level,
    decimal Amount,
    DateTimeOffset AccruedAt);
