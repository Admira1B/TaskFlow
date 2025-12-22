using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Comment.CreateComment {
    public record CreateCommentCommand(
        Guid TaskId,
        string Content,
        Guid AuthorId
    ) : IRequest<RequestResult<Unit>>;
}
