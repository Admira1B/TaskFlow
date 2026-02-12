using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;

namespace TaskFlow.Identity.Application.Commands.Auth.Logout {
    public class LogoutCommandHandler(ILogger logger, SignInManager<Domain.Entities.User> signInManager) : IRequestHandler<LogoutCommand, Unit> {
        private readonly ILogger _logger = logger;
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken = default) {
            await _signInManager.SignOutAsync();
            _logger.Debug("User logout successfully");

            return Unit.Value;
        }
    }
}
