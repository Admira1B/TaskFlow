using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Application.Commands.Auth.Logout {
    public class LogoutCommandHandler(SignInManager<Domain.Entities.User> signInManager) : IRequestHandler<LogoutCommand> {
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;


        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken) {
            await _signInManager.SignOutAsync();
        }
    }
}
