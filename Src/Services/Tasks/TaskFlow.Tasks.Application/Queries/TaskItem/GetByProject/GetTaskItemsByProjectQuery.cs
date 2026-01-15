using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByProject {
    public record GetTaskItemsByProjectQuery(
        Guid ProjectId    
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
