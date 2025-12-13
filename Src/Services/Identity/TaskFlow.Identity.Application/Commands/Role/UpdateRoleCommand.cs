namespace TaskFlow.Identity.Application.Commands.Role {
    public record UpdateRoleCommand (
        Guid Id,
        string Description
    );
}
