namespace PartnerCommissions.Users.Application.Models;

public record UserDetails(
    string ExternalId,
    string? ReferrerExternalId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ReferrerSetAt);
