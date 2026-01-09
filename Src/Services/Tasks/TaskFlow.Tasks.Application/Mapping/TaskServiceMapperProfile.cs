using AutoMapper;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.DTOs.Requests.Comment;
using TaskFlow.Tasks.Application.DTOs.Requests.Project;
using TaskFlow.Tasks.Application.DTOs.Requests.ProjectMember;
using TaskFlow.Tasks.Application.DTOs.Requests.TaskGroup;
using TaskFlow.Tasks.Application.DTOs.Requests.TaskItem;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Application.Commands.Comment.UpdateComment;
using TaskFlow.Tasks.Application.Commands.Project.CreateProject;
using TaskFlow.Tasks.Application.Commands.Project.UpdateProject;
using TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember;
using TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember;
using TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup;
using TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup;
using TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem;
using TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem;

namespace TaskFlow.Tasks.Application.Mapping {
    public class TaskServiceMapperProfile : Profile {
        public TaskServiceMapperProfile() {
            // POCOs to Response DTOs
            CreateMap<Comment, CommentDto>();
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<TaskGroup, TaskGroupDto>();
            CreateMap<TaskItem, TaskItemDto>();

            // Request DTOs to MediatoR commands
            CreateMap<CreateCommentRequest, CreateCommentCommand>()
                .ForMember(
                    dest => dest.AuthorId,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items["CurrentUserId"])
                )
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));
            CreateMap<UpdateCommentRequest, UpdateCommentCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateCommentCommand.Id)])
                )
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<CreateProjectRequest, CreateProjectCommand>();
            CreateMap<UpdateProjectRequest, UpdateProjectCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateProjectCommand.Id)])
                )
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<CreateProjectMemberRequest, CreateProjectMemberCommand>();
            CreateMap<UpdateProjectMemberRequest, UpdateProjectMemberCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateProjectMemberCommand.Id)])
                )
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<CreateTaskGroupRequest, CreateTaskGroupCommand>();
            CreateMap<UpdateTaskGroupRequest, UpdateTaskGroupCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateTaskGroupCommand.Id)])
                )
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CreateTaskItemRequest, CreateTaskItemCommand>()
                .ForMember(
                    dest => dest.ReporterId,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items["CurrentUserId"])
                );
            CreateMap<UpdateTaskItemRequest, UpdateTaskItemCommand>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom((src, dest, destMember, context)
                        => (Guid)context.Items[nameof(UpdateTaskItemCommand.Id)])
                )
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AssignedId, opt => opt.MapFrom(src => src.AssignedId))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId));
        }
    }
}
