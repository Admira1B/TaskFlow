using Microsoft.Extensions.Options;
using TaskFlow.Identity.Domain;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Services;

namespace TaskFlow.Identity.Application.Services {
    public class JsonWebTokenService(IOptions<JsonWebTokenOptions> options) : IJsonWebTokenService {
        private readonly JsonWebTokenOptions _options = options.Value;
        public async Task<string> GenerateWebToken(User user) {
            throw new NotImplementedException();
        }
    }
}
