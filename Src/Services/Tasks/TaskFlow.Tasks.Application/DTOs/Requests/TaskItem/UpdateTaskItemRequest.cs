using System.ComponentModel.DataAnnotations;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs.Requests.TaskItem {
    public record UpdateTaskItemRequest(
        [Required, MinLength(3), MaxLength(200)] string Title,
        [MaxLength(1000)] string? Description,
        [Required] Priority Priority,
        Guid? AssignedId,
        Guid? GroupId = null
    );
}
