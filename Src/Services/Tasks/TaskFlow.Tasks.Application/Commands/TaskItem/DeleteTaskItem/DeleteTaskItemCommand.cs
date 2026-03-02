using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public record DeleteTaskItemCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
