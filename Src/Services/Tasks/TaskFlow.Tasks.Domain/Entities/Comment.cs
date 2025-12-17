using TaskFlow.Shared.Core.Entities;

namespace TaskFlow.Tasks.Domain.Entities {
    public class Comment : EntityBase {
        public string Content { get; set; } = string.Empty;
        public Guid TaskId { get; set; }
        public Guid AuthorId { get; set; }

        // Navigation props
        public TaskItem? Task { get; set; }
    }
}
