using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByIdWithTasks {
    public record GetTaskGroupWithTasksByIdQuery(
        Guid Id        
    ) : IRequest<RequestResult<TaskGroupDto>>;
}
