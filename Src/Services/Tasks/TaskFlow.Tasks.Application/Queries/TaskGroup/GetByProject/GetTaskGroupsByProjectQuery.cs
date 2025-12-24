using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByProject {
    public record GetTaskGroupsByProjectQuery(
        Guid ProjectId
    ) : IRequest<RequestResult<List<TaskGroupDto>>>;
}
