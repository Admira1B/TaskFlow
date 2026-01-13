using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetById {
    public record GetTaskGroupByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<TaskGroupDto>>;
}
