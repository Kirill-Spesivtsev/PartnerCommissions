using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Application.Exceptions;
using PartnerCommissions.Commissions.Application.Models;
using PartnerCommissions.Commissions.Domain.Entities;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Application.Services;

public class EventService(
    IEventRepository events,
    ISchemeRepository schemes,
    IUsersClient usersClient,
    IEnumerable<CommissionScheme> commissionSchemes)
{
    public async Task<(EventDetails Event, bool Created)> CreateAsync(
        string externalId,
        string userExternalId,
        decimal profit,
        CancellationToken cancellationToken)
    {
        var ancestors = await usersClient.GetAncestorsAsync(userExternalId, cancellationToken);
        var settings = await schemes.GetCurrentAsync(cancellationToken);
        var scheme = commissionSchemes.Single(s => s.Type == settings.CurrentScheme);

        var profitEvent = new ProfitEvent
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            UserExternalId = userExternalId,
            Profit = profit,
            ReceivedAt = DateTimeOffset.UtcNow
        };

        profitEvent.Commissions.AddRange(scheme.Calculate(profitEvent, ancestors));

        if (await events.TryAddAsync(profitEvent, cancellationToken))
            return (ToEventDetails(profitEvent), true);

        var existing = await events.GetByIdAsync(externalId, cancellationToken);

        if (existing.UserExternalId != userExternalId || existing.Profit != profit)
            throw new ConflictException($"Event '{externalId}' already exists with different data.");

        return (ToEventDetails(existing), false);
    }

    public async Task<EventDetails> GetByIdAsync(string externalId, CancellationToken cancellationToken)
    {
        var profitEvent = await events.GetByIdAsync(externalId, cancellationToken);

        return ToEventDetails(profitEvent);
    }

    private static EventDetails ToEventDetails(ProfitEvent profitEvent)
    {
        var commissions = profitEvent.Commissions
            .OrderBy(c => c.Level)
            .Select(c => new CommissionDetails(c.PartnerExternalId, c.Level, c.Amount, c.SchemaType, c.AccruedAt, c.PaidAt))
            .ToList();

        return new EventDetails(profitEvent.ExternalId, profitEvent.UserExternalId, profitEvent.Profit, profitEvent.ReceivedAt, commissions);
    }
}
