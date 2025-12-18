using MediatR;
using TaskFlow.Tasks.Application.DTOs;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public record GetTaskItemByIdQuery(
        Guid Id
    ) : IRequest<TaskItemDto?>;
}
