using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetById {
    public record GetRoleByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<RoleDto>>;
}
