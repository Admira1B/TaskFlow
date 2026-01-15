using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByTaskGroup {
    public record GetTaskItemsByTaskGroupQuery(
        Guid TaskGroupId
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
