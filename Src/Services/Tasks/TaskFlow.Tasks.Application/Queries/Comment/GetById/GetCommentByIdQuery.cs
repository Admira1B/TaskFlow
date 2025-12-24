using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetById {
    public record GetCommentByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<CommentDto>>;
}
