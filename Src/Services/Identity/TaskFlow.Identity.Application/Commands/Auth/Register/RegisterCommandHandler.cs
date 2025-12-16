using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.Auth.Register {
    public class RegisterCommandHandler(IMapper mapper, UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager) : IRequestHandler<RegisterCommand, AuthResult> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager = signInManager;

        public async Task<AuthResult> Handle(RegisterCommand command, CancellationToken cancellationToken) {
            var existsByEmail = await _userManager.FindByEmailAsync(command.Email);

            if (existsByEmail is not null) {
                return AuthResult.Failure($"User with Email {command.Email} already exists");
            }

            var existsByName = await _userManager.FindByNameAsync(command.UserName);

            if (existsByName is not null) {
                return AuthResult.Failure($"User with UserName {command.UserName} already exists");
            }

            var user = new Domain.Entities.User {
                Email = command.Email,
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);

                return AuthResult.Failure(string.Join(",", errors));
            }

            return AuthResult.Success(_mapper.Map<UserDto>(user));
        }
    }
}
