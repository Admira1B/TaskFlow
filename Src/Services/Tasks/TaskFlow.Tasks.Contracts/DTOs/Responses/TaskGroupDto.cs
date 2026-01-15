namespace TaskFlow.Tasks.Contracts.DTOs.Responses {
    public record TaskGroupDto(
        Guid Id,
        string Name,
        Guid ProjectId,
        int TaskCount);
}
