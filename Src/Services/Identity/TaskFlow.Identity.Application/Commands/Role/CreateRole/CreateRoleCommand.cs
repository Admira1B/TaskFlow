using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public record CreateRoleCommand (
        string Name,
        string Description = ""
    ) : IRequest<RequestResult<RoleDto>>;
}
