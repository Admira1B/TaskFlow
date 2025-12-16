using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.Role;

namespace TaskFlow.Identity.Application.Queries.Role.GetByName {
    public class GetRoleByNameQueryHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<GetRoleByNameQuery, RoleDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RoleDto?> Handle(GetRoleByNameQuery query, CancellationToken cancellationToken) {
            var role = await _manager.FindByNameAsync(query.Name);

            if (role is null) {
                return null;
            }

            return _mapper.Map<RoleDto>(role);
        }
    }
}
