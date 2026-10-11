using PartnerCommissions.Commissions.Domain.Entities;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Application.Abstractions;

public interface ISchemeRepository
{
    Task<CommissionSettings> GetCurrentAsync(CancellationToken cancellationToken);
    Task SetAsync(SchemaType scheme, DateTimeOffset updatedAt, CancellationToken cancellationToken);
}
