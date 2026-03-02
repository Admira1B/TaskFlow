using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Domain.Contracts;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public class GetUsersPaginatedQueryHandler(IMapper mapper, IUserRepository repository) : IRequestHandler<GetUsersPaginatedQuery, RequestResult<IEnumerable<UserDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;

        public async Task<RequestResult<IEnumerable<UserDto>>> Handle(GetUsersPaginatedQuery query, CancellationToken cancellationToken = default) {
            var users = await _repository.GetPaginatedAsync(query.Page, query.PageSize, cancellationToken);

            return RequestResult<IEnumerable<UserDto>>.Success(users.Select(user => _mapper.Map<UserDto>(user)));
        }
    }
}
