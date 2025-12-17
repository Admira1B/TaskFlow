using TaskFlow.Shared.Core.Entities;

namespace TaskFlow.Tasks.Domain.Entities {
    public class Project : EntityBase {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid OwnerId { get; set; }

        // Navigation props
        public List<TaskGroup> Groups { get; set; } = [];
        public List<ProjectMember> Members { get; set; } = [];
    }
}
