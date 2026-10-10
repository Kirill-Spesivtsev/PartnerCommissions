using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Domain.Schemes;

public abstract class CommissionScheme
{
    public const int MaxLevels = 9;

    public abstract SchemaType Type { get; }

    public IReadOnlyList<Commission> Calculate(ProfitEvent profitEvent, IReadOnlyList<string> ancestors)
    {
        if (profitEvent.Profit <= 0)
            return [];

        return ancestors
            .Take(MaxLevels)
            .Select((partner, index) =>
            {
                var level = index + 1;
                return new Commission
                {
                    Id = Guid.NewGuid(),
                    EventId = profitEvent.Id,
                    PartnerExternalId = partner,
                    Level = level,
                    SchemaType = Type,
                    Amount = GetPercent(level) * profitEvent.Profit / 100m,
                    AccruedAt = profitEvent.ReceivedAt
                };
            })
            .ToList();
    }

    protected abstract decimal GetPercent(int level);
}
