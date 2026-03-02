using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.Comment.CreateComment {
    public record CreateCommentCommand(
        Guid TaskId,
        string Content,
        Guid AuthorId
    ) : IRequest<RequestResult<CommentDto>>;
}
