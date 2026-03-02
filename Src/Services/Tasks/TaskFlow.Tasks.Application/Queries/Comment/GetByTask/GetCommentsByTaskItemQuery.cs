using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetByTask {
    public record GetCommentsByTaskItemQuery(
        Guid TaskItemId
    ) : IRequest<RequestResult<List<CommentDto>>>;
}
