using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByName {
    public record GetUserByUserNameQuery (
        string UserName
    ) : IRequest<RequestResult<UserDto>>;
}
