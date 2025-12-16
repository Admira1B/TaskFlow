using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.Role;

namespace TaskFlow.Identity.Application.Queries.Role.GetById {
    public class GetRoleByIdQueryHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<GetRoleByIdQuery, RoleDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RoleDto?> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken) {
            var role = await _manager.FindByIdAsync(query.Id.ToString());

            if (role is null) {
                return null;
            }

            return _mapper.Map<RoleDto>(role);
        }
    }
}
