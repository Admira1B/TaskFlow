using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public record CreateTaskGroupCommand(
        Guid ProjectId,
        string Name
    ) : IRequest<RequestResult<TaskGroupDto>>;
}
