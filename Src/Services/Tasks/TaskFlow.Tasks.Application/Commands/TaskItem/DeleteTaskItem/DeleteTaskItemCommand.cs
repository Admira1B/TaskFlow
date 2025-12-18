using MediatR;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public record DeleteTaskItemCommand(Guid Id) : IRequest<bool>;
}
