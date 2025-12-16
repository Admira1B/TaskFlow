using MediatR;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public record DeleteUserCommand (
        Guid Id    
    ) : IRequest<bool>;
}
