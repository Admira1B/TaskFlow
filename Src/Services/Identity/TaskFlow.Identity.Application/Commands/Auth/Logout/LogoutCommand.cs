using MediatR;

namespace TaskFlow.Identity.Application.Commands.Auth.Logout {
    public record LogoutCommand : IRequest;
}
