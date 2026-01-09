using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Application.DTOs.Requests.Comment {
    public record UpdateCommentRequest(
        [Required, MinLength(1), MaxLength(2000)] string Content
    );
}
