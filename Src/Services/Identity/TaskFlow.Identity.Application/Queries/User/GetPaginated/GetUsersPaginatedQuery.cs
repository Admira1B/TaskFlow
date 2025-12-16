using MediatR;
using TaskFlow.Identity.Application.DTOs.User;

namespace TaskFlow.Identity.Application.Queries.User.GetPaginated {
    public record GetUsersPaginatedQuery (
        int Page,
        int PageSize
    ) : IRequest<IEnumerable<UserDto>>;
}
