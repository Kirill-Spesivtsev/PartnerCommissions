using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Domain.Tests.Schemes;

public class CommissionSchemeTests
{
    public static TheoryData<CommissionScheme> Schemes => new() { new LinearScheme(), new FibonacciScheme() };

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_ZeroProfit_ReturnsNoCommissions(CommissionScheme scheme)
    {
        var commissions = scheme.Calculate(TestEvents.WithProfit(0m), ["p1", "p2"]);

        Assert.Empty(commissions);
    }

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_NegativeProfit_ReturnsNoCommissions(CommissionScheme scheme)
    {
        var commissions = scheme.Calculate(TestEvents.WithProfit(-50m), ["p1", "p2"]);

        Assert.Empty(commissions);
    }

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_NoAncestors_ReturnsNoCommissions(CommissionScheme scheme)
    {
        var commissions = scheme.Calculate(TestEvents.WithProfit(100m), []);

        Assert.Empty(commissions);
    }

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_AssignsLevelsAndPartnersInAncestorOrder(CommissionScheme scheme)
    {
        var commissions = scheme.Calculate(TestEvents.WithProfit(100m), ["p1", "p2", "p3"]);

        Assert.Equal(["p1", "p2", "p3"], commissions.Select(c => c.PartnerExternalId));
        Assert.Equal([1, 2, 3], commissions.Select(c => c.Level));
    }

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_MoreThanNineAncestors_PaysOnlyNineLevels(CommissionScheme scheme)
    {
        var ancestors = Enumerable.Range(1, 12).Select(i => $"p{i}").ToList();

        var commissions = scheme.Calculate(TestEvents.WithProfit(100m), ancestors);

        Assert.Equal(CommissionScheme.MaxLevels, commissions.Count);
        Assert.Equal("p9", commissions[^1].PartnerExternalId);
    }

    [Theory]
    [MemberData(nameof(Schemes))]
    public void Calculate_TakesEventIdAndAccrualTimeFromEvent(CommissionScheme scheme)
    {
        var profitEvent = TestEvents.WithProfit(100m);

        var commissions = scheme.Calculate(profitEvent, ["p1", "p2"]);

        Assert.All(commissions, c =>
        {
            Assert.Equal(profitEvent.Id, c.EventId);
            Assert.Equal(profitEvent.ReceivedAt, c.AccruedAt);
            Assert.NotEqual(Guid.Empty, c.Id);
        });
    }

    [Fact]
    public void Calculate_FractionalProfit_KeepsExactDecimalAmounts()
    {
        var commissions = new FibonacciScheme().Calculate(TestEvents.WithProfit(0.01m), ["p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9"]);

        Assert.Equal(0.0001m, commissions[0].Amount);
        Assert.Equal(0.0034m, commissions[8].Amount);
        Assert.Equal(0.0088m, commissions.Sum(c => c.Amount));
    }
}
