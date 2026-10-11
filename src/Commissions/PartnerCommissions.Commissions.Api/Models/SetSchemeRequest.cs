using System.ComponentModel.DataAnnotations;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Api.Models;

public record SetSchemeRequest
{
    [Required]
    [EnumDataType(typeof(SchemaType))]
    public required SchemaType Scheme { get; init; }
}
