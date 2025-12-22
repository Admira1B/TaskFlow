using AutoMapper;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Entities;

namespace TaskFlow.Tasks.Application.Mapping {
    public class TasksMapperProfile : Profile {
        public TasksMapperProfile() {
            CreateMap<Comment, CommentDto>();

            CreateMap<Project, ProjectDto>();

            CreateMap<ProjectMember, ProjectMemberDto>();
            
            CreateMap<TaskGroup, TaskGroupDto>();
            
            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
