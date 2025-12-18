using MediatR;
using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Queries.Role.GetById {
    public record GetRoleByIdQuery (
        Guid Id
    ) : IRequest<RoleDto?>;
}
