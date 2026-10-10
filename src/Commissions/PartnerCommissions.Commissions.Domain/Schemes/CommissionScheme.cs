using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Domain.Schemes;

public abstract class CommissionScheme
{
    public const int MaxLevels = 9;

    public abstract SchemaType Type { get; }

    public IReadOnlyList<Commission> Calculate(decimal profit, IReadOnlyList<string> ancestors)
    {
        if (profit <= 0)
            return [];

        return ancestors
            .Take(MaxLevels)
            .Select((partner, index) =>
            {
                var level = index + 1;
                return new Commission
                {
                    PartnerExternalId = partner,
                    Level = level,
                    SchemaType = Type,
                    Amount = GetPercent(level) * profit / 100m
                };
            })
            .ToList();
    }

    protected abstract decimal GetPercent(int level);
}
