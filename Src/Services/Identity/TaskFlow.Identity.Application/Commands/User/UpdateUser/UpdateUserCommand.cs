using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Identity.Application.Commands.User.UpdateUser {
    public record UpdateUserCommand (
        Guid Id,
        string FirstName,
        string LastName
    ) : IRequest<RequestResult<Unit>>;
}
