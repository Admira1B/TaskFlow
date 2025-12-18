using AutoMapper;
using MediatR;
using TaskFlow.Identity.Application.DTOs;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Queries.Role.GetPaginated {
    public class GetRolesPaginatedQueryHandler(IMapper mapper, IRoleRepository repository) : IRequestHandler<GetRolesPaginatedQuery, IEnumerable<RoleDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IRoleRepository _repository = repository;

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesPaginatedQuery query, CancellationToken cancellationToken) {
            var roles = await _repository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);

            return roles.Select(role => _mapper.Map<RoleDto>(role));
        }
    }
}
