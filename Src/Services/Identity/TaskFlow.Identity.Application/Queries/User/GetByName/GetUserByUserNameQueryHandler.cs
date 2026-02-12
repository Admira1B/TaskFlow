using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByName {
    public class GetUserByUserNameQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByUserNameQuery, RequestResult<UserDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<RequestResult<UserDto>> Handle(GetUserByUserNameQuery query, CancellationToken cancellationToken = default) {
            var user = await _manager.FindByNameAsync(query.UserName);

            if (user is null) {
                return RequestResult<UserDto>.NotFound("User");
            }

            return RequestResult<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
    }
}
