using MediatR;
using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Queries.Role.GetByName {
    public record GetRoleByNameQuery (
        string Name
    ) : IRequest<RoleDto?>;
}
