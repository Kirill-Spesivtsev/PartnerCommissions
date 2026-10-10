using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Domain.Tests.Schemes;

public class FibonacciSchemeTests
{
    private readonly FibonacciScheme _scheme = new();

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    [InlineData(7, 13)]
    [InlineData(8, 21)]
    [InlineData(9, 34)]
    public void Calculate_ProfitOf100_GivesFibonacciPercentAtEachLevel(int level, int expectedAmount)
    {
        var ancestors = Ancestors(level);

        var commissions = _scheme.Calculate(100m, ancestors);

        Assert.Equal(expectedAmount, commissions[level - 1].Amount);
    }

    [Fact]
    public void Calculate_FullChainOfNine_PaysEightyEightPercentInTotal()
    {
        var commissions = _scheme.Calculate(1000m, Ancestors(9));

        Assert.Equal(9, commissions.Count);
        Assert.Equal(880m, commissions.Sum(c => c.Amount));
    }

    [Fact]
    public void Calculate_SetsFibonacciSchemaTypeOnEveryCommission()
    {
        var commissions = _scheme.Calculate(100m, Ancestors(9));

        Assert.All(commissions, c => Assert.Equal(SchemaType.Fibonacci, c.SchemaType));
    }

    private static IReadOnlyList<string> Ancestors(int count)
    {
        return Enumerable.Range(1, count).Select(i => $"p{i}").ToList();
    }
}
