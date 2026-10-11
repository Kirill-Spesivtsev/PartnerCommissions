using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Application.Models;
using PartnerCommissions.Commissions.Domain.Schemes;

namespace PartnerCommissions.Commissions.Application.Services;

public class SchemeService(ISchemeRepository schemes)
{
    public async Task<SchemeDetails> GetCurrentAsync(CancellationToken cancellationToken)
    {
        var settings = await schemes.GetCurrentAsync(cancellationToken);

        return new SchemeDetails(settings.CurrentScheme, settings.UpdatedAt);
    }

    public async Task SetAsync(SchemaType scheme, CancellationToken cancellationToken)
    {
        var settings = await schemes.GetCurrentAsync(cancellationToken);

        if (settings.CurrentScheme == scheme)
            return;

        await schemes.SetAsync(scheme, DateTimeOffset.UtcNow, cancellationToken);
    }
}
