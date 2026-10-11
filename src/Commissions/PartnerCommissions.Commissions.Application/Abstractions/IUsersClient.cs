namespace PartnerCommissions.Commissions.Application.Abstractions;

public interface IUsersClient
{
    Task<IReadOnlyList<string>> GetAncestorsAsync(string userExternalId, CancellationToken cancellationToken);
}
