using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByProject {
    public record GetTaskItemsByProjectQuery(
        Guid ProjectId    
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
