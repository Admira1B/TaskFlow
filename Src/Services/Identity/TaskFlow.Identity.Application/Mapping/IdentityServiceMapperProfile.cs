using AutoMapper;
using TaskFlow.Identity.Application.DTOs;
using TaskFlow.Identity.Domain.Entities;

namespace TaskFlow.Identity.Application.Mapping {
    public class IdentityServiceMapperProfile : Profile {
        public IdentityServiceMapperProfile() {
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
        }
    }
}
