using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByTaskGroup {
    public record GetTaskItemsByTaskGroupQuery(
        Guid TaskGroupId
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
