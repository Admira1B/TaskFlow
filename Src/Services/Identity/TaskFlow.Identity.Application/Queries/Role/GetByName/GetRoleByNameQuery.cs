using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetByName {
    public record GetRoleByNameQuery (
        string Name
    ) : IRequest<RequestResult<RoleDto>>;
}
