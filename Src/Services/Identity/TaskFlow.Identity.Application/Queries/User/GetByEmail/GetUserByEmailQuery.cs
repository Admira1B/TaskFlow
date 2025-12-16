using MediatR;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public record GetUserByEmailQuery (
        string Email
    ) : IRequest<UserDto?>;
}
