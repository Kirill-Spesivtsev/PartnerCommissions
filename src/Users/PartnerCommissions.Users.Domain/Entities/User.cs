namespace PartnerCommissions.Users.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string ExternalId { get; set; }

    public Guid? ReferrerId { get; set; }
    public User? Referrer { get; set; }
    public List<User> Referrals { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ReferrerSetAt { get; set; }
}
