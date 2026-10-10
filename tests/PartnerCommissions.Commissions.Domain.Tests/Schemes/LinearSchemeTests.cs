using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Domain.Tests.Schemes;

public class LinearSchemeTests
{
    private readonly LinearScheme _scheme = new();

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 4)]
    [InlineData(5, 5)]
    [InlineData(6, 6)]
    [InlineData(7, 7)]
    [InlineData(8, 8)]
    [InlineData(9, 9)]
    public void Calculate_ProfitOf100_GivesLevelPercentAtEachLevel(int level, int expectedAmount)
    {
        var ancestors = Ancestors(level);

        var commissions = _scheme.Calculate(TestEvents.WithProfit(100m), ancestors);

        Assert.Equal(expectedAmount, commissions[level - 1].Amount);
    }

    [Fact]
    public void Calculate_FullChainOfNine_PaysFortyFivePercentInTotal()
    {
        var commissions = _scheme.Calculate(TestEvents.WithProfit(1000m), Ancestors(9));

        Assert.Equal(9, commissions.Count);
        Assert.Equal(450m, commissions.Sum(c => c.Amount));
    }

    [Fact]
    public void Calculate_SetsLinearSchemaTypeOnEveryCommission()
    {
        var commissions = _scheme.Calculate(TestEvents.WithProfit(100m), Ancestors(9));

        Assert.All(commissions, c => Assert.Equal(SchemaType.Linear, c.SchemaType));
    }

    private static IReadOnlyList<string> Ancestors(int count)
    {
        return Enumerable.Range(1, count).Select(i => $"p{i}").ToList();
    }
}
