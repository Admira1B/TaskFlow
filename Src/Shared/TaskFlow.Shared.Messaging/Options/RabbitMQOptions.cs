namespace TaskFlow.Shared.Messaging.Options {
    public class RabbitMqOptions {
        public required string UserName { get; init; }
        public required string Password { get; init; }
        public required string VirtualHost { get; init; }
        public required string HostName { get; init; }
        public int Port { get; init; }
    }
}
