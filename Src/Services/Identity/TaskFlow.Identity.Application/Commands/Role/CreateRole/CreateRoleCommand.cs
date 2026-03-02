using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public record CreateRoleCommand (
        string Name,
        string Description = ""
    ) : IRequest<RequestResult<RoleDto>>;
}
