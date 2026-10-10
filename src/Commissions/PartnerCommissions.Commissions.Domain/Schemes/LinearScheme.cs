namespace PartnerCommissions.Commissions.Domain.Schemes;

public class LinearScheme : CommissionScheme
{
    public override SchemaType Type => SchemaType.Linear;

    protected override decimal GetPercent(int level) => level;
}
