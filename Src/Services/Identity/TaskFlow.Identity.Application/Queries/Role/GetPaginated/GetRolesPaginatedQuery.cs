using MediatR;
using TaskFlow.Identity.Application.DTOs.Role;

namespace TaskFlow.Identity.Application.Queries.Role.GetPaginated {
    public record GetRolesPaginatedQuery(
        int Page,
        int PageSize
    ) : IRequest<IEnumerable<RoleDto>>;
}
