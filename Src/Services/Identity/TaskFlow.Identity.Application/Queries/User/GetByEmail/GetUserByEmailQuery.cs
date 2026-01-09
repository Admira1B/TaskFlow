using MediatR;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public record GetUserByEmailQuery (
        string Email
    ) : IRequest<UserDto?>;
}
