using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public record GetTaskItemByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<TaskItemDto>>;
}
