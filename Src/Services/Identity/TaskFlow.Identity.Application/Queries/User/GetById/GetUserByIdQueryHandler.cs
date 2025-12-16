using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetById {
    public class GetUserByIdQueryHandler(IMapper mapper, UserManager<Domain.Entities.User> manager) : IRequestHandler<GetUserByIdQuery, UserDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<UserDto?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken) {
            var user = await _manager.FindByIdAsync(query.Id.ToString());

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
