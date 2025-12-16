using AutoMapper;
using MediatR;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public class GetUsersPaginatedQueryHandler(IMapper mapper, IUserRepository repository) : IRequestHandler<GetUsersPaginatedQuery, IEnumerable<UserDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        
        public async Task<IEnumerable<UserDto>> Handle(GetUsersPaginatedQuery query, CancellationToken cancellationToken) {
            var users = await _repository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);

            return users.Select(user => _mapper.Map<UserDto>(user));
        }
    }
}
