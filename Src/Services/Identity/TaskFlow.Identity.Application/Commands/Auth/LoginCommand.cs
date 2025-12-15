namespace TaskFlow.Identity.Application.Commands.Auth {
    public record LoginCommand (
        string UserName, 
        string Password
    );
}
