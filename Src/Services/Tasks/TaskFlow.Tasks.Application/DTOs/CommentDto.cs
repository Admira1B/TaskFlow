namespace TaskFlow.Tasks.Application.DTOs {
    public record CommentDto(
        Guid Id,
        string Content,
        Guid AuthorId,
        Guid TaskId,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
