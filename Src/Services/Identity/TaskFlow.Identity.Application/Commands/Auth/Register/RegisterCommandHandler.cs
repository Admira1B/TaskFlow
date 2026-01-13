using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Domain.Contracts.Services;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Auth.Register {
    public class RegisterCommandHandler(IMapper mapper, UserManager<Domain.Entities.User> userManager, IJsonWebTokenService jwtService) : IRequestHandler<RegisterCommand, AuthResult> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _userManager = userManager;
        private readonly IJsonWebTokenService _jwtService = jwtService;

        public async Task<AuthResult> Handle(RegisterCommand command, CancellationToken cancellationToken) {
            var existsByEmail = await _userManager.FindByEmailAsync(command.Email);

            if (existsByEmail is not null) {
                return AuthResult.AlreadyExists("Email", command.Email);
            }

            var existsByName = await _userManager.FindByNameAsync(command.UserName);

            if (existsByName is not null) {
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

                return AuthResult.Failure(string.Join(",", errors));
            }

            var token = await _jwtService.GenerateWebTokenAsync(user);

            return AuthResult.Success(_mapper.Map<UserDto>(user), token);
        }
    }
}
