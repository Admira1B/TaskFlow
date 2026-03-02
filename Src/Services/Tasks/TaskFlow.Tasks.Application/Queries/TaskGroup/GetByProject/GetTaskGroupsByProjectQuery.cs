using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByProject {
    public record GetTaskGroupsByProjectQuery(
        Guid ProjectId
    ) : IRequest<RequestResult<List<TaskGroupDto>>>;
}
