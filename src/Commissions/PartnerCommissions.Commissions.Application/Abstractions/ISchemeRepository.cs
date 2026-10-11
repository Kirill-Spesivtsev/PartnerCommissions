using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Application.Abstractions;

public interface ISchemeRepository
{
    Task<CommissionSettings> GetCurrentAsync(CancellationToken cancellationToken);
}
