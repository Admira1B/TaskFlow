namespace TaskFlow.Identity.Application.Commands.Auth {
    public record RegisterCommand (
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password
    );
}
