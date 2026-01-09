using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Application.DTOs.Requests.Comment {
    public record CreateCommentRequest(
        [Required, MinLength(1), MaxLength(2000)] string Content,
        [Required] Guid TaskId
    );
}
