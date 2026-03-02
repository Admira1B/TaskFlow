using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.Exists {
    public record UserExistsQuery(
        Guid UserId    
    ) : IRequest<RequestResult<ExistenceResponse>>;
}
