using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public record GetUsersPaginatedQuery (
        int Page,
        int PageSize
    ) : IRequest<RequestResult<IEnumerable<UserDto>>>;
}
