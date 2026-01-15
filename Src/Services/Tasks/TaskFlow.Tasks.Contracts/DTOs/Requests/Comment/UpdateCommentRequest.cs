using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Tasks.Contracts.DTOs.Requests.Comment {
    public record UpdateCommentRequest(
        [Required, MinLength(1), MaxLength(2000)] string Content
    );
}
