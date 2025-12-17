using TaskFlow.Shared.Core.Entities;

namespace TaskFlow.Tasks.Domain.Entities {
    public class TaskGroup : EntityBase {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        // Navigation props
        public Project? Project { get; set; }
        public List<TaskItem> TaskItems { get; set; } = [];
    }
}
