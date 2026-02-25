namespace TaskFlow.Shared.Core.Abstractions {
    public abstract class EventBase {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public string EventType => this.GetType().Name;
        public string SourceService { get; set; } = string.Empty;
    }
}
