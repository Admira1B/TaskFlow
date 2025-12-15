namespace TaskFlow.Identity.Application.Commands.User {
    public record CreateUserCommand (
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password
    );
}
