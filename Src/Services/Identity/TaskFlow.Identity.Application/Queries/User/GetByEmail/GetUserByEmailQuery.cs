using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public record GetUserByEmailQuery (
        string Email
    ) : IRequest<RequestResult<UserDto>>;
}
