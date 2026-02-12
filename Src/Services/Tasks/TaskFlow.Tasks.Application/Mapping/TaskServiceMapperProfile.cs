using AutoMapper;
using TaskFlow.Tasks.Domain.Entities;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Contracts.DTOs.Requests.Comment;
using TaskFlow.Tasks.Contracts.DTOs.Requests.Project;
using TaskFlow.Tasks.Contracts.DTOs.Requests.ProjectMember;
using TaskFlow.Tasks.Contracts.DTOs.Requests.TaskGroup;
using TaskFlow.Tasks.Contracts.DTOs.Requests.TaskItem;
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

            // ========= Request DTOs to MediatoR commands =========

            // Comment maps
            CreateMap<CreateCommentRequest, CreateCommentCommand>()
                .ConstructUsing((src, context) => new CreateCommentCommand(
                    TaskId: src.TaskId,
                    Content: src.Content,
                    AuthorId: (Guid)context.Items[nameof(CreateCommentCommand.AuthorId)]
                ));

            CreateMap<UpdateCommentRequest, UpdateCommentCommand>()
                .ConstructUsing((src, context) => new UpdateCommentCommand(
                    Id: (Guid)context.Items[nameof(UpdateCommentCommand.Id)],
                    Content: src.Content
                ));

            // Project maps
            CreateMap<CreateProjectRequest, CreateProjectCommand>()
                .ConstructUsing((src, context) => new CreateProjectCommand(
                    Name: src.Name,
                    Description: src.Description,
                    OwnerId: (Guid)context.Items[nameof(CreateProjectCommand.OwnerId)]
                ));

            CreateMap<UpdateProjectRequest, UpdateProjectCommand>()
                .ConstructUsing((src, context) => new UpdateProjectCommand(
                    Id: (Guid)context.Items[nameof(UpdateProjectCommand.Id)],
                    Name: src.Name,
                    Description: src.Description,
                    IsActive: src.IsActive
                ));

            // ProjectMember maps
            CreateMap<CreateProjectMemberRequest, CreateProjectMemberCommand>();

            CreateMap<UpdateProjectMemberRequest, UpdateProjectMemberCommand>()
                .ConstructUsing((src, context) => new UpdateProjectMemberCommand(
                    Id: (Guid)context.Items[nameof(UpdateProjectMemberCommand.Id)],
                    Role: src.Role
                ));

            // TaskGroup maps
            CreateMap<CreateTaskGroupRequest, CreateTaskGroupCommand>();

            CreateMap<UpdateTaskGroupRequest, UpdateTaskGroupCommand>()
                .ConstructUsing((src, context) => new UpdateTaskGroupCommand(
                    Id: (Guid)context.Items[nameof(UpdateTaskGroupCommand.Id)],
                    Name: src.Name
                ));

            // Task maps
            CreateMap<CreateTaskItemRequest, CreateTaskItemCommand>()
                .ConstructUsing((src, context) => new CreateTaskItemCommand(
                    Title: src.Title,
                    Description: src.Description,
                    GroupId: src.GroupId,
                    ReporterId: (Guid)context.Items[nameof(CreateTaskItemCommand.ReporterId)],
                    AssignedId: src.AssignedId,
                    Priority: src.Priority
                ));

            CreateMap<UpdateTaskItemRequest, UpdateTaskItemCommand>()
                .ConstructUsing((src, context) => new UpdateTaskItemCommand(
                    Id: (Guid)context.Items[nameof(UpdateTaskItemCommand.Id)],
                    Title: src.Title,
                    Description: src.Description,
                    GroupId: src.GroupId,
                    AssignedId: src.AssignedId,
                    Priority: src.Priority
                ));
        }
    }
}
