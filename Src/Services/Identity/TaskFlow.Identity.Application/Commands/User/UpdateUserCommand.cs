namespace TaskFlow.Identity.Application.Commands.User {
    public record UpdateUserCommand (
        Guid Id,
        string FirstName,
        string LastName
    );
}
