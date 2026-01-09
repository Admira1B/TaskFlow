using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Application.DTOs.Requests.TaskGroup {
    public record CreateTaskGroupRequest(
        [Required, MinLength(3), MaxLength(50)] string Name,
        [Required] Guid ProjectId
    );
}
