using PartnerCommissions.Users.Application.Models;
using PartnerCommissions.Users.Domain.Entities;

namespace PartnerCommissions.Users.Application.Abstractions;

public interface IUserRepository
{
    Task<UserDetails?> GetDetailsAsync(string externalId, CancellationToken cancellationToken);
    Task<bool> TryAddAsync(User user, CancellationToken cancellationToken);
}
