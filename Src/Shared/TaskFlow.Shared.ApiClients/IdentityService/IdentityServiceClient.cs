using System.Net;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Shared.ApiClients.IdentityService {
    public class IdentityServiceClient(ILogger logger, HttpClient httpClient) : ServiceClientBase(logger, httpClient) {
        public async Task<ExistenceResponse> UserExistsAsync(Guid userId, CancellationToken ct = default) {
            string endpoint = $"flow/users/exists/{userId}";

            try {
                var result = await ExecuteGetAsync<ExistenceResponse>(endpoint, ct).ConfigureAwait(false);
                return result ?? new ExistenceResponse(Exists: false);
            } catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound) {
                return new ExistenceResponse(Exists: false);
            }
        }
    }
}
