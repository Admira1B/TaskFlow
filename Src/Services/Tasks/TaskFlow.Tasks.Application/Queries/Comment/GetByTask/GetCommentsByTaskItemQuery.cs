using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetByTask {
    public record GetCommentsByTaskItemQuery(
        Guid TaskItemId
    ) : IRequest<RequestResult<List<CommentDto>>>;
}
