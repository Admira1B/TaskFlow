using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetById {
    public record GetUserByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<UserDto>>;
}
