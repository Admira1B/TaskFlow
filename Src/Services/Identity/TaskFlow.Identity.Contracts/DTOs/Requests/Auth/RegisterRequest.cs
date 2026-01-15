using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Contracts.DTOs.Requests.Auth {
    public record RegisterRequest(
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(3, ErrorMessage = "UserName must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "UserName cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "UserName can only contain letters, numbers and underscores")]
        string UserName,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        string Email,

        [Required(ErrorMessage = "FirstName is required")]
        [MinLength(2, ErrorMessage = "FirstName must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "FirstName cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "FirstName can only contain letters, spaces and hyphens")]
        string FirstName,

        [Required(ErrorMessage = "LastName is required")]
        [MinLength(2, ErrorMessage = "LastName must be at least 2 characters")]
        [MaxLength(50, ErrorMessage = "LastName cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s\-]+$", ErrorMessage = "LastName can only contain letters, spaces and hyphens")]
        string LastName,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        string Password
    );
}
