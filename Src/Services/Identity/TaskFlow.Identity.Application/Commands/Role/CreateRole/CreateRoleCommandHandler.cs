using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public class CreateRoleCommandHandler(ILogger logger, IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<CreateRoleCommand, RequestResult<RoleDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;
        
        public async Task<RequestResult<RoleDto>> Handle(CreateRoleCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Role creation attempt with name {RoleName}", command.Name);
            var existingRole = await _manager.FindByNameAsync(command.Name);

            if (existingRole is not null) {
                _logger.Debug("Role creation failed. Role with name {RoleName} already exists", command.Name);
                return RequestResult<RoleDto>.AlreadyExists("Role", existingRole.Id.ToString());
            }
            
            var role = new Domain.Entities.Role {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _manager.CreateAsync(role);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(",", errors);

                _logger.Debug("Role creation failed. Name: {RoleName}, Error: {Error}", command.Name, message);
                return RequestResult<RoleDto>.Failure(message);
            }

            _logger.Debug("Role created successfully. Name: {RoleName}, Description: {Description}", command.Name, command.Description);

            return RequestResult<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }
    }
}
