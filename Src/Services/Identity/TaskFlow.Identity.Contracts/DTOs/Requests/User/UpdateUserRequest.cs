using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Contracts.DTOs.Requests.User {
    public record UpdateUserRequest(
        [Required(ErrorMessage = "FirstName is required")]
        [MinLength(2, ErrorMessage = "FirstName must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "FirstName can only contain letters, spaces and hyphens")]
        string FirstName,

        [Required(ErrorMessage = "LastName is required")]
        [MinLength(2, ErrorMessage = "LastName must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "LastName can only contain letters, spaces and hyphens")]
        string LastName
    );
}
