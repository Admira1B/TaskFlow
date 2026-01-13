using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public record GetTaskItemByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<TaskItemDto>>;
}
