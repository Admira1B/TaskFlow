using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public class GetUserByEmailQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByEmailQuery, RequestResult<UserDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<RequestResult<UserDto>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken) {
            var user = await _manager.FindByEmailAsync(query.Email);

            if (user is null) {
                return RequestResult<UserDto>.NotFound("User");
            }

            return RequestResult<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
    }
}
