using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetByName {
    public class GetRoleByNameQueryHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<GetRoleByNameQuery, RequestResult<RoleDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RequestResult<RoleDto>> Handle(GetRoleByNameQuery query, CancellationToken cancellationToken) {
            var role = await _manager.FindByNameAsync(query.Name);

            if (role is null) {
                return RequestResult<RoleDto>.NotFound("Role");
            }

            return RequestResult<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }
    }
}
