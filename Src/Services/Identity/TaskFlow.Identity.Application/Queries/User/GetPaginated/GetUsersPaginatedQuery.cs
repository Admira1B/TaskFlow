using MediatR;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public record GetUsersPaginatedQuery (
        int Page,
        int PageSize
    ) : IRequest<RequestResult<IEnumerable<UserDto>>>;
}
