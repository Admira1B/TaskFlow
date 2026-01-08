using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByReporter {
    public record GetTaskItemsByReporterQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<TaskItemDto>>>;
}
