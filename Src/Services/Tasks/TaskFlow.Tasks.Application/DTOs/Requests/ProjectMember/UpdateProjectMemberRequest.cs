using System.ComponentModel.DataAnnotations;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs.Requests.ProjectMember {
    public record UpdateProjectMemberRequest(
        [Required] ProjectRole Role
    );
}
