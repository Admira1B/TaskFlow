using System.ComponentModel.DataAnnotations;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Contracts.DTOs.Requests.ProjectMember {
    public record CreateProjectMemberRequest(
        [Required] Guid ProjectId,
        [Required] Guid UserId,
        ProjectRole Role = ProjectRole.Member
    );
}
