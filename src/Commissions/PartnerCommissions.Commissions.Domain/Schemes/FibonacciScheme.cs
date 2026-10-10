namespace PartnerCommissions.Commissions.Domain.Schemes;

public class FibonacciScheme : CommissionScheme
{
    public override SchemaType Type => SchemaType.Fibonacci;

    protected override decimal GetPercent(int level)
    {
        var previous = 0;
        var current = 1;

        for (var i = 1; i < level; i++)
        {
            var next = previous + current;
            previous = current;
            current = next;
        }

        return current;
    }
}
