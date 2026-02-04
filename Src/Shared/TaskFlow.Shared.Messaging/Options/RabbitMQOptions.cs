using System.Text.Json.Serialization;

namespace TaskFlow.Shared.Messaging.Options {
    public class RabbitMqOptions {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
        public string HostName { get; set; } = null!;

        [JsonIgnore]
        public int PortParsed => int.Parse(Port);
    }
}
