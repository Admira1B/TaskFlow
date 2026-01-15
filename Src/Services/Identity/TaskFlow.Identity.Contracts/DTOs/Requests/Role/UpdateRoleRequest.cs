using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Contracts.DTOs.Requests.Role {
    public record UpdateRoleRequest(
        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        string Description
    );
}
