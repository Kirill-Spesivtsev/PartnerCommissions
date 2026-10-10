using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Domain.Entities;

public class CommissionSettings
{
    public int Id { get; set; }
    public SchemaType CurrentScheme { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
