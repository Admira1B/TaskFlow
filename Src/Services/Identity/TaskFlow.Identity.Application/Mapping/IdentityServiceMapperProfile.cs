using AutoMapper;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Contracts.DTOs.Responses;
using TaskFlow.Identity.Contracts.DTOs.Requests.Auth;
using TaskFlow.Identity.Contracts.DTOs.Requests.Role;
using TaskFlow.Identity.Contracts.DTOs.Requests.User;
using TaskFlow.Identity.Application.Commands.Auth.Login;
using TaskFlow.Identity.Application.Commands.Auth.Register;
using TaskFlow.Identity.Application.Commands.Role.CreateRole;
using TaskFlow.Identity.Application.Commands.Role.UpdateRole;
using TaskFlow.Identity.Application.Commands.User.UpdateUser;
using TaskFlow.Identity.Application.Queries.Role.GetPaginated;
using TaskFlow.Identity.Application.Queries.User.GetPaginated;

namespace TaskFlow.Identity.Application.Mapping {
    public class IdentityServiceMapperProfile : Profile {
        public IdentityServiceMapperProfile() {
            // POCOs to Response DTOs
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();

            // Request DTOs to MediatoR commands
            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<RegisterRequest, RegisterCommand>();

            CreateMap<GetRolesPaginatedRequest, GetRolesPaginatedQuery>();
            CreateMap<CreateRoleRequest, CreateRoleCommand>();
            CreateMap<UpdateRoleRequest, UpdateRoleCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateRoleCommand.Id)])
                )
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<GetUsersPaginatedRequest, GetUsersPaginatedQuery>();
            CreateMap<UpdateUserRequest, UpdateUserCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateUserCommand.Id)])
                )
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}
