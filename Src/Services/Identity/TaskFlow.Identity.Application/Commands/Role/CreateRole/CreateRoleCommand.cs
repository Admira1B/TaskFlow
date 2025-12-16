using MediatR;
using TaskFlow.Identity.Application.DTOs.Role;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public record CreateRoleCommand (
        string Name,
        string Description = ""
    ) : IRequest<RoleDto?>;
}
