using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByProject {
    public record GetTaskItemsByProjectQuery(
        Guid ProjectId    
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
