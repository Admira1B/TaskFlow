using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Identity.Application.Commands.Role.UpdateRole {
    public record UpdateRoleCommand (
        Guid Id,
        string Description
    ) : IRequest<RequestResult<Unit>>;
}
