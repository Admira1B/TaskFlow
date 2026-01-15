using System.ComponentModel.DataAnnotations;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Contracts.DTOs.Requests.ProjectMember {
    public record UpdateProjectMemberRequest(
        [Required] ProjectRole Role
    );
}
