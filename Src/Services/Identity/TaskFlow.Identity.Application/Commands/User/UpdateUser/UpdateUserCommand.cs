using MediatR;

namespace TaskFlow.Identity.Application.Commands.User.UpdateUser {
    public record UpdateUserCommand (
        Guid Id,
        string FirstName,
        string LastName
    ) : IRequest<bool>;
}
