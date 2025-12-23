using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Comment.DeleteComment {
    public record DeleteCommentCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
