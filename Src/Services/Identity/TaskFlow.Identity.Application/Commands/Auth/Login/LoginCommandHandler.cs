using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Auth.Login {
    public class LoginCommandHandler(IMapper mapper, UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager) : IRequestHandler<LoginCommand, AuthResult> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;
        public async Task<AuthResult> Handle(LoginCommand command, CancellationToken cancellationToken) {
            var user = await _userManager.FindByNameAsync(command.UserName);

            if (user == null) {
                return AuthResult.Failure("Invalid credentials");
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                command.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) {
                return AuthResult.Failure("Invalid credentials");
            }

            return AuthResult.Success(_mapper.Map<UserDto>(user));
        }
    }
}
