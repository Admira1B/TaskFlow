using MediatR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetById {
    public class GetUserByIdQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByIdQuery, RequestResult<UserDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<RequestResult<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default) {
            var user = await _manager.FindByIdAsync(query.Id.ToString());

            if (user is null) {
                return RequestResult<UserDto>.NotFound("User", query.Id);
            }

            return RequestResult<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
    }
}
