using System.Net.Http.Json;
using PartnerCommissions.Wallets.Application.Abstractions;
using PartnerCommissions.Wallets.Application.Exceptions;
using PartnerCommissions.Wallets.Application.Models;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace PartnerCommissions.Wallets.Infrastructure.Clients;

public class CommissionsClient(HttpClient httpClient) : ICommissionsClient
{
    public async Task<IReadOnlyList<UnpaidCommission>> GetUnpaidAsync(int limit, CancellationToken cancellationToken)
    {
        using var response = await SendAsync(() => httpClient.GetAsync($"api/v1/commissions/unpaid?limit={limit}", cancellationToken));

        var commissions = await response.Content.ReadFromJsonAsync<List<UnpaidCommission>>(cancellationToken);

        return commissions ?? [];
    }

    public async Task MarkPaidAsync(IReadOnlyCollection<Guid> commissionIds, DateTimeOffset paidAt, CancellationToken cancellationToken)
    {
        var request = new { CommissionIds = commissionIds, PaidAt = paidAt };

        using var response = await SendAsync(() => httpClient.PostAsJsonAsync("api/v1/commissions/mark-paid", request, cancellationToken));
    }

    private static async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> send)
    {
        HttpResponseMessage response;

        try
        {
            response = await send();
        }
        catch (Exception ex) when (ex is HttpRequestException or BrokenCircuitException or TimeoutRejectedException)
        {
            throw new ServiceUnavailableException("Commissions service is unavailable.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            response.Dispose();
            throw new ServiceUnavailableException($"Commissions service responded with {(int)response.StatusCode}.");
        }

        return response;
    }
}
