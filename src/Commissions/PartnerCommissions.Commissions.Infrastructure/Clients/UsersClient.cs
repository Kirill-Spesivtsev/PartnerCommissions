using System.Net;
using System.Net.Http.Json;
using PartnerCommissions.Commissions.Application.Abstractions;
using PartnerCommissions.Commissions.Application.Exceptions;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace PartnerCommissions.Commissions.Infrastructure.Clients;

public class UsersClient(HttpClient httpClient) : IUsersClient
{
    public async Task<IReadOnlyList<string>> GetAncestorsAsync(string userExternalId, CancellationToken cancellationToken)
    {
        HttpResponseMessage response;

        try
        {
            response = await httpClient.GetAsync($"api/v1/users/{Uri.EscapeDataString(userExternalId)}/tree/up", cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or BrokenCircuitException or TimeoutRejectedException)
        {
            throw new ServiceUnavailableException("Users service is unavailable.", ex);
        }

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new UserNotFoundException($"User '{userExternalId}' was not found.");

            if (!response.IsSuccessStatusCode)
                throw new ServiceUnavailableException($"Users service responded with {(int)response.StatusCode}.");

            var ancestors = await response.Content.ReadFromJsonAsync<List<string>>(cancellationToken);

            return ancestors ?? [];
        }
    }
}
