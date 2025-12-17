using TaskFlow.Shared.Core.Entities;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Domain.Entities {
    public class TaskItem : EntityBase {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Priority Priority { get; set; } = 0;
        public Guid GroupId { get; set; }
        public Guid ReporterId { get; set; }
        public Guid? AssignedId { get; set; }

        // Navigation props
        public TaskGroup? Group { get; set; }
        public List<Comment> Comments { get; set; } = [];
    }
}
