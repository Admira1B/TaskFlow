namespace TaskFlow.Identity.Application.DTOs {
    public record UserDto(
        Guid Id,
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
