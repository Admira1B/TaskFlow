using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetPaginated {
    public record GetRolesPaginatedQuery(
        int Page,
        int PageSize
    ) : IRequest<RequestResult<IEnumerable<RoleDto>>>;
}
