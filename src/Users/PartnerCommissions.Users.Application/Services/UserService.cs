using PartnerCommissions.Users.Application.Abstractions;
using PartnerCommissions.Users.Application.Exceptions;
using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Application.Services;

public class UserService(IUserRepository users)
{
    public async Task<UserDetails> CreateAsync(string externalId, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        if (!await users.TryAddAsync(user, cancellationToken))
            throw new ConflictException($"User '{externalId}' already exists.");

        return new UserDetails(user.ExternalId, null, user.CreatedAt, null);
    }

    public Task<UserDetails> GetByIdAsync(string externalId, CancellationToken cancellationToken)
    {
        return users.GetDetailsAsync(externalId, cancellationToken);
    }

    public Task<IReadOnlyList<string>> GetAncestorsAsync(string externalId, CancellationToken cancellationToken)
    {
        return users.GetAncestorsAsync(externalId, cancellationToken);
    }

    public Task<IReadOnlyList<ReferralTreeNode>> GetDescendantsAsync(string externalId, int maxDepth, CancellationToken cancellationToken)
    {
        return users.GetDescendantsAsync(externalId, maxDepth, cancellationToken);
    }
}
