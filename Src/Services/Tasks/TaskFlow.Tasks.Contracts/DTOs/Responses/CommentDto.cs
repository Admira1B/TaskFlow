namespace TaskFlow.Tasks.Contracts.DTOs.Responses {
    public record CommentDto(
        Guid Id,
        string Content,
        Guid AuthorId,
        Guid TaskId,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
