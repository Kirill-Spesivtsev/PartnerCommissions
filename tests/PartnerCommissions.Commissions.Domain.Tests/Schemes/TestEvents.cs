using PartnerCommissions.Commissions.Domain.Entities;

namespace PartnerCommissions.Commissions.Domain.Tests.Schemes;

public static class TestEvents
{
    public static ProfitEvent WithProfit(decimal profit)
    {
        return new ProfitEvent
        {
            Id = Guid.NewGuid(),
            ExternalId = "e1",
            UserExternalId = "u1",
            Profit = profit,
            ReceivedAt = new DateTimeOffset(2026, 10, 1, 12, 0, 0, TimeSpan.Zero)
        };
    }
}
