namespace TaskFlow.Identity.Application.Commands.Role {
    public record CreateRoleCommand (
        string Name,
        string Description = ""
    );
}
