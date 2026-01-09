using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Application.DTOs.Requests.Role {
    public record CreateRoleRequest(
        [Required(ErrorMessage = "Role name is required")]
        [MinLength(3, ErrorMessage = "Role name must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Role name can only contain letters and spaces")]
        string Name,

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        string Description = ""
    );
}
