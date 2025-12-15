using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Application.Queries.User;
using TaskFlow.Identity.Application.Commands.User;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;
using AutoMapper;

namespace TaskFlow.Identity.Application.Services {
    public class UserService(IMapper mapper, IUserRepository repository, UserManager<User> manager) {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _repository = repository;
        private readonly UserManager<User> _manager = manager;

        public async Task<UserDto?> GetByIdAsync(GetUserByIdQuery query) {
            var user = await _manager.FindByIdAsync(query.Id.ToString());

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByEmailAsync(GetUserByEmailQuery query) {
            var user = await _manager.FindByEmailAsync(query.Email);

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByUserNameAsync(GetUserByUserNameQuery query) {
            var user = await _manager.FindByNameAsync(query.UserName);

            if (user is null) {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetPaginatedAsync(GetUsersPaginatedQuery query) {
            var users = await _repository.GetPaginatedAsync(query.Page, query.PageSize);

            return users.Select(user => _mapper.Map<UserDto>(user));
        }

        public async Task<bool> UpdateAsync(UpdateUserCommand command) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return false;
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _manager.UpdateAsync(user);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(DeleteUserCommand command) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return false;
            }

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }
    }
}
