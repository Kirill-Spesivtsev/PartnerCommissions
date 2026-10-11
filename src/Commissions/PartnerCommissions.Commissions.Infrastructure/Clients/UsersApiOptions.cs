namespace PartnerCommissions.Commissions.Infrastructure.Clients;

public class UsersApiOptions
{
    public const string SectionName = "Services:UsersApi";

    public string BaseUrl { get; set; } = "http://users-api:8080/";
}
