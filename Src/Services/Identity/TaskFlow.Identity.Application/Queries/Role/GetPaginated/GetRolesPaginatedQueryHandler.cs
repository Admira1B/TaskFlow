using MediatR;
using AutoMapper;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Queries.Role.GetPaginated {
    public class GetRolesPaginatedQueryHandler(IMapper mapper, IRoleRepository repository) : IRequestHandler<GetRolesPaginatedQuery, RequestResult<IEnumerable<RoleDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IRoleRepository _repository = repository;

        public async Task<RequestResult<IEnumerable<RoleDto>>> Handle(GetRolesPaginatedQuery query, CancellationToken cancellationToken) {
            var roles = await _repository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);

            return RequestResult<IEnumerable<RoleDto>>.Success(roles.Select(role => _mapper.Map<RoleDto>(role)));
        }
    }
}
