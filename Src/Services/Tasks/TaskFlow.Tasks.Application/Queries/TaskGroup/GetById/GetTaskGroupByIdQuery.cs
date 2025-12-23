using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetById {
    public record GetTaskGroupByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<TaskGroupDto>>;
}
