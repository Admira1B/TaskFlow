using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Contracts.DTOs.Requests.Project {
    public record CreateProjectRequest(
        [Required, MinLength(3), MaxLength(100)] string Name,
        [MaxLength(500)] string? Description
    );
}
