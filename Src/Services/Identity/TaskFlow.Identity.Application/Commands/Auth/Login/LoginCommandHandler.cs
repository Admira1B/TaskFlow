using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Auth.Login {
    public class LoginCommandHandler(ILogger logger, IMapper mapper, UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager, JsonWebTokenService jwtService) : IRequestHandler<LoginCommand, AuthResult> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;
        private readonly JsonWebTokenService _jwtService = jwtService;

        public async Task<AuthResult> Handle(LoginCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Login attempt for Username: {UserName}", command.UserName);
            var user = await _userManager.FindByNameAsync(command.UserName);

            if (user == null) {
                _logger.Debug("Login failed. User not found: {UserName}", command.UserName);
                return AuthResult.NotFound("UserName", command.UserName);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                command.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) {
                _logger.Debug("Login failed for User: {UserName}", command.UserName);
                return AuthResult.InvalidCredentials();
            }

            var token = await _jwtService.GenerateWebTokenAsync(user);

            _logger.Debug("User {UserId} logged in successfully", user.Id.ToString());
            return AuthResult.Success(_mapper.Map<UserDto>(user), token);
        }
    }
}
