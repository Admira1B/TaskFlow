using System.ComponentModel.DataAnnotations;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs.Requests.ProjectMember {
    public record CreateProjectMemberRequest(
        [Required] Guid ProjectId,
        [Required] Guid UserId,
        ProjectRole Role = ProjectRole.Member
    );
}
