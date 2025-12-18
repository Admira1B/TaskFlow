using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Queries.User.GetByEmail {
    public class GetUserByEmailQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByEmailQuery, UserDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<UserDto?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken) {
            var user = await _manager.FindByEmailAsync(query.Email);

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
