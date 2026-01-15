using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Domain.Contracts.Services;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Auth.Login {
    public class LoginCommandHandler(IMapper mapper, UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager, IJsonWebTokenService jwtService) : IRequestHandler<LoginCommand, AuthResult> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;
        private readonly IJsonWebTokenService _jwtService = jwtService;

        public async Task<AuthResult> Handle(LoginCommand command, CancellationToken cancellationToken) {
            var user = await _userManager.FindByNameAsync(command.UserName);

            if (user == null) {
                return AuthResult.NotFound("UserName", command.UserName);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                command.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) {
                return AuthResult.InvalidCredentials();
            }

            var token = await _jwtService.GenerateWebTokenAsync(user);

            return AuthResult.Success(_mapper.Map<UserDto>(user), token);
        }
    }
}
