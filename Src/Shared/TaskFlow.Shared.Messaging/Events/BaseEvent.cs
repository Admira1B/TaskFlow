using System.Text.Json.Serialization;

namespace TaskFlow.Shared.Messaging.Events {
    public abstract class BaseEvent {
        [JsonPropertyName("id")]
        public Guid EventId { get; set; } = Guid.NewGuid();
        [JsonPropertyName("occurredOn")]
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("type")]
        public string EventType => this.GetType().Name;
        [JsonPropertyName("source")]
        public string SourceService { get; set; } = string.Empty;
    }
}
