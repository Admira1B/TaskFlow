using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Contracts.DTOs.Requests.Auth {
    public record LoginRequest(
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        string UserName,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        string Password
    );
}
