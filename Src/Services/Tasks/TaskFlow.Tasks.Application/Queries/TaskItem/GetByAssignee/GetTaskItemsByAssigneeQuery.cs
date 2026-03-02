using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByAssignee {
    public record GetTaskItemsByAssigneeQuery(
        Guid UserId        
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
