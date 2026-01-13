using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.Role.GetById {
    public class GetRoleByIdQueryHandler(IMapper mapper, RoleManager<Domain.Entities.Role> manager) : IRequestHandler<GetRoleByIdQuery, RequestResult<RoleDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<Domain.Entities.Role> _manager = manager;

        public async Task<RequestResult<RoleDto>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken) {
            var role = await _manager.FindByIdAsync(query.Id.ToString());

            if (role is null) {
                return RequestResult<RoleDto>.NotFound("Role", query.Id);
            }

            return RequestResult<RoleDto>.Success(_mapper.Map<RoleDto>(role));
        }
    }
}
