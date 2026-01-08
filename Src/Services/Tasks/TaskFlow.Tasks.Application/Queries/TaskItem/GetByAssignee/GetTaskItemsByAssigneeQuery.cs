using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByAssignee {
    public record GetTaskItemsByAssigneeQuery(
        Guid UserId        
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
