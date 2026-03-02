using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public record GetTaskItemByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<TaskItemDto>>;
}
