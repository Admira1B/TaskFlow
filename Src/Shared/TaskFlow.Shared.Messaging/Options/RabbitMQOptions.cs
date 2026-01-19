namespace TaskFlow.Shared.Messaging.Options {
    public class RabbitMqOptions {
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public int Port { get; set; } = 5672;
        public string VirtualHost { get; set; } = "/";
        public string HostName { get; set; } = "localhost";
    }
}
