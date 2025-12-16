using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.Role;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public class CreateRoleCommandHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<CreateRoleCommand, RoleDto?> {
        private readonly IMapper _mapper = mapper;   
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;
        
        public async Task<RoleDto?> Handle(CreateRoleCommand command, CancellationToken cancellationToken) {
            var role = new Domain.Entities.Role {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _manager.CreateAsync(role);

            if (!result.Succeeded) {
                return null;
            }

            return _mapper.Map<RoleDto>(role);
        }
    }
}
