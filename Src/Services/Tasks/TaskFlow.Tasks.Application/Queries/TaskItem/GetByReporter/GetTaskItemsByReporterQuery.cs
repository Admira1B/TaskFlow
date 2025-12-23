using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByReporter {
    public record GetTaskItemsByReporterQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
