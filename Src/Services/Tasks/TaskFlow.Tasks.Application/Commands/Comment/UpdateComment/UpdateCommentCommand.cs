using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Comment.UpdateComment {
    public record UpdateCommentCommand (
        Guid Id,
        string Content
    ) : IRequest<RequestResult<Unit>>;
}
