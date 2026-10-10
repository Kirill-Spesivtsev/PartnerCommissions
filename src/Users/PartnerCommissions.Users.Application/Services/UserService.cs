using System.Data;
using PartnerCommissions.Users.Application.Abstractions;
using PartnerCommissions.Users.Application.Exceptions;
using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Application.Services;

public class UserService(IUserRepository users, IUnitOfWork unitOfWork)
{
    private const int MaxChainLength = 10;

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

    public async Task<UserDetails> GetByIdAsync(string externalId, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(externalId, cancellationToken);

        return new UserDetails(user.ExternalId, user.Referrer?.ExternalId, user.CreatedAt, user.ReferrerSetAt);
    }

    public Task<IReadOnlyList<string>> GetAncestorsAsync(string externalId, CancellationToken cancellationToken)
    {
        return users.GetAncestorsAsync(externalId, cancellationToken);
    }

    public Task<IReadOnlyList<ReferralTreeNode>> GetDescendantsAsync(string externalId, int maxDepth, CancellationToken cancellationToken)
    {
        return users.GetDescendantsAsync(externalId, maxDepth, cancellationToken);
    }

    public async Task SetReferrerAsync(string externalId, string referrerExternalId, CancellationToken cancellationToken)
    {
        var user = await users.GetByIdAsync(externalId, cancellationToken);
        var referrer = await users.GetByIdAsync(referrerExternalId, cancellationToken);

        await EnsureCanAttachAsync(user, referrer, cancellationToken);

        user.ReferrerId = referrer.Id;
        user.ReferrerSetAt = DateTimeOffset.UtcNow;

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureCanAttachAsync(User user, User referrer, CancellationToken cancellationToken)
    {
        if (user.Id == referrer.Id)
            throw new ConflictException($"User '{user.ExternalId}' cannot be their own referrer.");

        var referrerAncestors = await users.GetAncestorsAsync(referrer.ExternalId, cancellationToken);

        if (referrerAncestors.Contains(user.ExternalId))
            throw new ConflictException($"Attaching '{user.ExternalId}' to '{referrer.ExternalId}' would create a cycle.");

        var userDescendants = await users.GetDescendantsAsync(user.ExternalId, MaxChainLength - 1, cancellationToken);

        var referrerDepth = referrerAncestors.Count + 1;
        var userSubtreeHeight = GetHeight(userDescendants) + 1;
        var chainLength = referrerDepth + userSubtreeHeight;

        if (chainLength > MaxChainLength)
            throw new ConflictException($"Referral chain would become {chainLength} long, max is {MaxChainLength}.");
    }

    private static int GetHeight(IReadOnlyList<ReferralTreeNode> nodes)
    {
        return nodes.Count == 0 ? 0 : 1 + nodes.Max(n => GetHeight(n.Referrals));
    }
}
