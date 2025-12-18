using TaskFlow.Shared.Core.Entities;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Domain.Entities {
    public class ProjectMember : EntityBase {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectRole? Role {
            get {
                return field;
            }

            set {
                if (value is null) {
                    field = ProjectRole.Member;
                }
            } 
        }

        // Navigation props
        public Project? Project { get; set; }
    }
}
