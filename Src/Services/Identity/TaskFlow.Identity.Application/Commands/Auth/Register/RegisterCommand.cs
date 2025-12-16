using MediatR;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Auth.Register {
    public record RegisterCommand (
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password
    ) : IRequest<AuthResult>;
}
