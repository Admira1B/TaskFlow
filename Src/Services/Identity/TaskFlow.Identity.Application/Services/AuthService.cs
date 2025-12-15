using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.Commands.Auth;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Services {
    public class AuthService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<User> _userManager = userManager; 
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<AuthResult> RegisterAsync(RegisterCommand command) {
            var existsByEmail = await _userManager.FindByEmailAsync(command.Email);

            if (existsByEmail is not null) {
                return AuthResult.Failure($"User with Email {command.Email} already exists");
            }

            var existsByName = await _userManager.FindByNameAsync(command.UserName);

            if (existsByName is not null) { 
                return AuthResult.Failure($"User with UserName {command.UserName} already exists");
            }

            var user = new User { 
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

        public async Task<AuthResult> LoginAsync(LoginCommand command) {
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

        public async Task LogoutAsync() {
            await _signInManager.SignOutAsync();
        }
    }
}
