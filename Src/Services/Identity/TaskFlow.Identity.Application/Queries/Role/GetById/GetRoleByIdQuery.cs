using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetById {
    public record GetRoleByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<RoleDto>>;
}
