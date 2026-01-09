using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Application.DTOs.Requests.TaskGroup {
    public record UpdateTaskGroupRequest(
        [Required, MinLength(3), MaxLength(50)] string Name
    );
}
