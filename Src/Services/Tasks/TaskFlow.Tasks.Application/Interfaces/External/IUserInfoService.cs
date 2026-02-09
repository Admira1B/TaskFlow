namespace TaskFlow.Tasks.Application.Interfaces.External {
    public interface IUserService {
        Task<bool> ExistsAsync(Guid userId, CancellationToken ct); 
    }
}
