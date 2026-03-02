using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetByName {
    public record GetRoleByNameQuery (
        string Name
    ) : IRequest<RequestResult<RoleDto>>;
}
