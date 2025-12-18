using MediatR;
using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Queries.User.GetById {
    public record GetUserByIdQuery (
        Guid Id
    ) : IRequest<UserDto?>;
}
