using TaskFlow.Shared.Core.Entities;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Domain.Entities {
    public class TaskItem : EntityBase {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid GroupId { get; set; }
        public Guid ReporterId { get; set; }
        public Guid? AssignedId { get; set; }
        public Priority? Priority {
            get {
                return field;
            } 
            set {
                if (value is null) { 
                    field = Enums.Priority.Medium;
                }
            } 
        }

        // Navigation props
        public TaskGroup? Group { get; set; }
        public List<Comment> Comments { get; set; } = [];

        public TaskItem(string title, Guid groupId, Guid reporterId, string? description = null, Guid? assignedId = null, Priority? priority = null) {
            Title = title;
            GroupId = groupId;
            ReporterId = reporterId;
            Description = description;
            AssignedId = assignedId;
            Priority = priority;
            Comments = [];
        }
    }
}
