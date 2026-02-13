using System.Net;
using Microsoft.AspNetCore.Http;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Shared.ApiClients.IdentityService {
    // TODO: Add Exceptions processing
    public class IdentityServiceClient(ILogger logger, HttpClient httpClient, IHttpContextAccessor accessor) : ServiceClientBase(logger, httpClient, accessor) {
        public async Task<ExistenceResponse> UserExistsAsync(Guid userId, CancellationToken ct = default) {
            string endpoint = $"/users/exists/{userId}";

            try {
                var result = await ExecuteGetAsync<ExistenceResponse>(endpoint, ct).ConfigureAwait(false);

                return result!;
            } catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.NotFound) {
                // route is wrong
                throw;
            } catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.Unauthorized) {
                // failed to authorize
                throw;
            } catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.Forbidden) {
                // no required role
                throw;
            } catch (Exception) {
                throw;
            }
        }
    }
}
