using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public record DeleteUserCommand (
        Guid Id    
    ) : IRequest<RequestResult<Unit>>;
}
