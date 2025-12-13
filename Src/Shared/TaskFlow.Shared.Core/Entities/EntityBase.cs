namespace TaskFlow.Shared.Core.Entities {
    public class EntityBase {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }

        public void MarkAsUpdated() {
            UpdatedAt = DateTime.UtcNow;
        }

        public override bool Equals(object? obj) {
            if (obj is null) {
                return false;
            }

            if (obj is not EntityBase other) {
                return false;
            }

            if (GetType() != other.GetType()) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode() { 
            return this.Id.GetHashCode();
        }
    }
}
