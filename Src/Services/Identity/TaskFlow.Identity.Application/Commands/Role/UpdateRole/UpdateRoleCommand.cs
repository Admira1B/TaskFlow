using MediatR;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Role.UpdateRole {
    public record UpdateRoleCommand (
        Guid Id,
        string Description
    ) : IRequest<RequestResult<Unit>>;
}
