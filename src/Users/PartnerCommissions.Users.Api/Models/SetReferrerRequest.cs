using System.ComponentModel.DataAnnotations;

namespace PartnerCommissions.Users.Api.Models;

public record SetReferrerRequest
{
    [Required]
    [MaxLength(64)]
    public required string ReferrerExternalId { get; init; }
}
