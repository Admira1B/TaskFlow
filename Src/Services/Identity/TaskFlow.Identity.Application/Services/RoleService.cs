using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;
using TaskFlow.Identity.Application.DTOs.Role;
using TaskFlow.Identity.Application.Queries.Role;
using TaskFlow.Identity.Application.Commands.Role;

namespace TaskFlow.Identity.Application.Services {
    public class RoleService(IRoleRepository repository, RoleManager<Role> manager) {
        private readonly IRoleRepository _repository = repository;
        private readonly RoleManager<Role> _manager = manager;

        public async Task<RoleDto?> GetByIdAsync(GetRoleByIdQuery query) {
            var role = await _manager.FindByIdAsync(query.Id.ToString());

            if (role is null) {
                return null;
            }

            return BuildDto(role);
        }

        public async Task<RoleDto?> GetByNameAsync(GetRoleByNameQuery query) {
            var role = await _manager.FindByNameAsync(query.Name);

            if (role is null) {
                return null;
            }

            return BuildDto(role);
        }

        public async Task<IEnumerable<RoleDto>> GetPaginatedAsync(GetRolesPaginatedQuery query) {
            var roles = await _repository.GetPaginatedAsync(query.Page, query.PageSize);

            return roles.Select(role => BuildDto(role));
        }

        public async Task<RoleDto?> CreateAsync(CreateRoleCommand command) {
            var role = new Role {
                Name = command.Name,
                Description = command.Description
            };

            var result = await _manager.CreateAsync(role);

            if (!result.Succeeded) {
                return null;
            }

            return BuildDto(role);
        }

        public async Task<bool> UpdateAsync(UpdateRoleCommand command) {
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                return false;
            }

            role.Description = command.Description;

            var result = await _manager.UpdateAsync(role);

            if (!result.Succeeded) { 
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(DeleteRoleCommand command) {
            var role = await _manager.FindByIdAsync(command.Id.ToString());

            if (role is null) {
                return false;
            }

            var result = await _manager.DeleteAsync(role);

            if (!result.Succeeded) { 
                return false;
            }

            return true;
        }

        // TODO: Later replace with AutoMapper
        private RoleDto BuildDto(Role role) {
            return new RoleDto(
                Id: role.Id,
                Name: role.Name ?? string.Empty,
                Description: role.Description ?? string.Empty,
                CreatedAt: role.CreatedAt,
                UpdatedAt: role.UpdatedAt
            );
        }
    }
}
