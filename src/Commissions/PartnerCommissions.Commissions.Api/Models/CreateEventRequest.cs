using System.ComponentModel.DataAnnotations;

namespace PartnerCommissions.Commissions.Api.Models;

public record CreateEventRequest : IValidatableObject
{
    [Required]
    [MaxLength(64)]
    public required string ExternalId { get; init; }

    [Required]
    [MaxLength(64)]
    public required string UserExternalId { get; init; }

    public required decimal Profit { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Profit != decimal.Round(Profit, 2))
            yield return new ValidationResult("Profit must have at most 2 decimal places.", [nameof(Profit)]);
    }
}
