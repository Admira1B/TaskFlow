namespace TaskFlow.Tasks.Application.DTOs {
    public record TaskGroupDto(
        Guid Id,
        string Name,
        Guid ProjectId,
        int TaskCount);
}
