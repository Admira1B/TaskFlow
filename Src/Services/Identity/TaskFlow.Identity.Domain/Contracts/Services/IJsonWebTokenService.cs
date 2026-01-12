using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Domain.Contracts.Services {
    public interface IJsonWebTokenService {
        Task<string> GenerateWebTokenAsync(User user);
    }
}
