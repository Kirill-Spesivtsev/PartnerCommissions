using System.ComponentModel.DataAnnotations;

namespace PartnerCommissions.Commissions.Api.Models;

public record MarkPaidRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public required List<Guid> CommissionIds { get; init; }

    public required DateTimeOffset PaidAt { get; init; }
}
