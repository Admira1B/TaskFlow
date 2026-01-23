using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.Services;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Auth.Register {
    public class RegisterCommandHandler(ILogger logger, IMapper mapper, UserManager<Domain.Entities.User> userManager, JsonWebTokenService jwtService) : IRequestHandler<RegisterCommand, AuthResult> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly JsonWebTokenService _jwtService = jwtService;

        public async Task<AuthResult> Handle(RegisterCommand command, CancellationToken cancellationToken) {
            _logger.Debug($"Registration attempt. Email: {command.Email}, Username: {command.UserName}");
            var existsByEmail = await _userManager.FindByEmailAsync(command.Email);

            if (existsByEmail is not null) {
                _logger.Debug($"Registration failed - email already exists: {command.Email}");
                return AuthResult.AlreadyExists("Email", command.Email);
            }

            var existsByName = await _userManager.FindByNameAsync(command.UserName);

            if (existsByName is not null) {
                _logger.Debug($"Registration failed - username already exists: {command.UserName}");
                return AuthResult.AlreadyExists("UserName", command.UserName);
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
                var message = string.Join(",", errors);

                _logger.Warn($"User creation failed. Email: {command.Email}, Errors: {message}");
                return AuthResult.Failure(message);
            }

            var token = await _jwtService.GenerateWebTokenAsync(user);

            _logger.Info($"User registered successfully. UserId: {user.Id}, Email: {user.Email}");
            _logger.Debug($"Registration details - UserId: {user.Id}, Name: {user.FirstName} {user.LastName}");

            return AuthResult.Success(_mapper.Map<UserDto>(user), token);
        }
    }
}
