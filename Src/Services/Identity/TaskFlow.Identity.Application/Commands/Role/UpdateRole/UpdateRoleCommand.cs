using MediatR;

namespace TaskFlow.Identity.Application.Commands.Role.UpdateRole {
    public record UpdateRoleCommand (
        Guid Id,
        string Description
    ) : IRequest<bool>;
}
