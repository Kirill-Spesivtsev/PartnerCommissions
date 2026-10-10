using System.ComponentModel.DataAnnotations;

namespace PartnerCommissions.Users.Api.Models;

public record CreateUserRequest
{
    [Required]
    [MaxLength(64)]
    public required string ExternalId { get; init; }
}
