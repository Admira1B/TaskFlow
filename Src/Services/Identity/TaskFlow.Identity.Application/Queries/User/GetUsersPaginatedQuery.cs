namespace TaskFlow.Identity.Application.Queries.User {
    public record GetUsersPaginatedQuery (
        int Page,
        int PageSize
    );
}
