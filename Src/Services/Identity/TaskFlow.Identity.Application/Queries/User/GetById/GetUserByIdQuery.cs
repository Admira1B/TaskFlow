using MediatR;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetById {
    public record GetUserByIdQuery (
        Guid Id
    ) : IRequest<UserDto?>;
}
