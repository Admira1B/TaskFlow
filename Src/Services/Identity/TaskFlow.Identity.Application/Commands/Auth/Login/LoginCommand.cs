using MediatR;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Auth.Login {
    public record LoginCommand (
        string UserName, 
        string Password
    ) : IRequest<AuthResult>;
}
