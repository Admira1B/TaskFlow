using MediatR;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public record DeleteUserCommand (
        Guid Id    
    ) : IRequest<RequestResult<Unit>>;
}
