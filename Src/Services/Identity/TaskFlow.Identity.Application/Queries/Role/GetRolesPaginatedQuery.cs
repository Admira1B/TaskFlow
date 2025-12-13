namespace TaskFlow.Identity.Application.Queries.Role {
    public record GetRolesPaginatedQuery(
        int Page,
        int PageSize
    );
}
