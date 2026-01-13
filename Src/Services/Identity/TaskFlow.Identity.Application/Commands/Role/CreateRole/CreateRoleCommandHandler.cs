using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Commands.Role.CreateRole {
    public class CreateRoleCommandHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<CreateRoleCommand, RequestResult<RoleDto>> {
        private readonly IMapper _mapper = mapper;   
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;
        
        public async Task<RequestResult<RoleDto>> Handle(CreateRoleCommand command, CancellationToken cancellationToken) {
            var existingRole = await _manager.FindByNameAsync(command.Name);

            if (existingRole is not null) {
                return RequestResult<RoleDto>.AlreadyExists("Role", existingRole.Id.ToString());
            }
            
            var role = new Domain.Entities.Role {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _manager.CreateAsync(role);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);

                return RequestResult<RoleDto>.Failure(string.Join(",", errors));
            }

            return RequestResult<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }
    }
}
