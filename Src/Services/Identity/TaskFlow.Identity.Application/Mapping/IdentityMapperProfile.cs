using AutoMapper;
using TaskFlow.Identity.Application.DTOs.Role;
using TaskFlow.Identity.Application.DTOs.User;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Application.Mapping {
    public class IdentityMapperProfile : Profile {
        public IdentityMapperProfile() {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
        }
    }
}
