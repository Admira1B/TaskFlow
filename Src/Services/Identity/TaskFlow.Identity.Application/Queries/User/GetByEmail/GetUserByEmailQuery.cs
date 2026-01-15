using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public record GetUserByEmailQuery (
        string Email
    ) : IRequest<RequestResult<UserDto>>;
}
