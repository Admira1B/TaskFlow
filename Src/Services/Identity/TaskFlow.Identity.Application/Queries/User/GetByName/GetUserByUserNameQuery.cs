using MediatR;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetByName {
    public record GetUserByUserNameQuery (
        string UserName
    ) : IRequest<UserDto?>;
}
