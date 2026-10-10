using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Application.Abstractions;

public interface IUserRepository
{
    Task<UserDetails?> GetDetailsAsync(string externalId, CancellationToken cancellationToken);
    Task<bool> TryAddAsync(User user, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetAncestorsAsync(string externalId, CancellationToken cancellationToken);
    Task<ReferralTreeNode?> GetDescendantsAsync(string externalId, int maxDepth, CancellationToken cancellationToken);
}
