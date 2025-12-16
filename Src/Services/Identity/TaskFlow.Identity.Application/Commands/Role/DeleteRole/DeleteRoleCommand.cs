using MediatR;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public record DeleteRoleCommand (
        Guid Id    
    ) : IRequest<bool>;
}
