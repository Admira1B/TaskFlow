using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public record DeleteRoleCommand (
        Guid Id    
    ) : IRequest<RequestResult<Unit>>;
}
