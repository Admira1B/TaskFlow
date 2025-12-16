using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetByName {
    public class GetUserByUserNameQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByUserNameQuery, UserDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<UserDto?> Handle(GetUserByUserNameQuery query, CancellationToken cancellationToken) {
            var user = await _manager.FindByNameAsync(query.UserName);

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
