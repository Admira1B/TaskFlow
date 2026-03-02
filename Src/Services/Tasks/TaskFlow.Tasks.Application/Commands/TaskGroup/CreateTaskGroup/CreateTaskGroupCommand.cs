using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public record CreateTaskGroupCommand(
        Guid ProjectId,
        string Name
    ) : IRequest<RequestResult<TaskGroupDto>>;
}
