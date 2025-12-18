using MediatR;
using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Queries.User.GetByName {
    public record GetUserByUserNameQuery (
        string UserName
    ) : IRequest<UserDto?>;
}
