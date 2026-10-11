namespace PartnerCommissions.Wallets.Infrastructure.Clients;

public class CommissionsApiOptions
{
    public const string SectionName = "Services:CommissionsApi";

    public string BaseUrl { get; set; } = "http://commissions-api:8080/";
}
