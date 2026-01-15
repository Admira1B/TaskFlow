using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetById {
    public record GetCommentByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<CommentDto>>;
}
