using MediatR;
using AutoMapper;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public class GetUsersPaginatedQueryHandler(IMapper mapper, IUserRepository repository) : IRequestHandler<GetUsersPaginatedQuery, RequestResult<IEnumerable<UserDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;

        public async Task<RequestResult<IEnumerable<UserDto>>> Handle(GetUsersPaginatedQuery query, CancellationToken cancellationToken) {
            var users = await _repository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);

            return RequestResult<IEnumerable<UserDto>>.Success(users.Select(user => _mapper.Map<UserDto>(user)));
        }
    }
}
