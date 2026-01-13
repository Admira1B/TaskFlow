using MediatR;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Role.DeleteRole {
    public record DeleteRoleCommand (
        Guid Id    
    ) : IRequest<RequestResult<Unit>>;
}
