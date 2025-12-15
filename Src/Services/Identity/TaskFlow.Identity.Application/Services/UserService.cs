using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Application.Queries.User;
using TaskFlow.Identity.Application.Commands.User;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Services {
    public class UserService(IUserRepository repository, UserManager<User> manager) {
        private readonly IUserRepository _repository = repository;
        private readonly UserManager<User> _manager = manager;

        public async Task<UserDto?> GetByIdAsync(GetUserByIdQuery query) {
            var user = await _manager.FindByIdAsync(query.Id.ToString());

            if (user is null) {
                return null;
            }

            return await BuildDto(user);
        }

        public async Task<UserDto?> GetByEmailAsync(GetUserByEmailQuery query) {
            var user = await _manager.FindByEmailAsync(query.Email);

            if (user is null) {
                return null;
            }

            return await BuildDto(user);
        }

        public async Task<UserDto?> GetByUserNameAsync(GetUserByUserNameQuery query) {
            var user = await _manager.FindByNameAsync(query.UserName);

            if (user is null) {
                return null;
            }

            return await BuildDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetPaginatedAsync(GetUsersPaginatedQuery query) {
            var users = await _repository.GetPaginatedAsync(query.Page, query.PageSize);

            var result = new List<UserDto>();

            foreach (var user in users) {
                var dto = await BuildDto(user);
                result.Add(dto);
            }

            return result;
        }

        public async Task<UserDto?> CreateAsync(CreateUserCommand command) {
            var user = new User {
                Email = command.Email,
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var result = await _manager.CreateAsync(user);

            if (!result.Succeeded) {
                return null;
            }

            return await BuildDto(user);
        }

        public async Task<bool> UpdateAsync(UpdateUserCommand command) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return false;
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;

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

        // TODO: Later replace with AutoMapper
        private async Task<UserDto> BuildDto(User user) {
            var roles = await _manager.GetRolesAsync(user);

            return new UserDto(
                Id: user.Id,
                UserName: user.UserName ?? string.Empty,
                Email: user.Email ?? string.Empty,
                FirstName: user.FirstName ?? string.Empty,
                LastName: user.LastName ?? string.Empty,
                CreatedAt: user.CreatedAt,
                UpdatedAt: user.UpdatedAt,
                Roles: roles);
        }
    }
}
