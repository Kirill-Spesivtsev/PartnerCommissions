using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Application.Abstractions;

public interface IEventRepository
{
    Task<ProfitEvent> GetByIdAsync(string externalId, CancellationToken cancellationToken);
    Task<bool> TryAddAsync(ProfitEvent profitEvent, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProfitEvent>> ListByUserAsync(string userExternalId, int page, int pageSize, CancellationToken cancellationToken);
}
