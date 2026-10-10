namespace PartnerCommissions.Users.Application.Models;

public record ReferralTreeNode(string ExternalId, IReadOnlyList<ReferralTreeNode> Referrals);
