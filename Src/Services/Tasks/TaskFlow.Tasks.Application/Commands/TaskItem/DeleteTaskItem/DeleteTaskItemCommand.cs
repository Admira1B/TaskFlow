using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public record DeleteTaskItemCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
