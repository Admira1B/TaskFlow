using System.Text.Json.Serialization;

namespace TaskFlow.Shared.Core.Options {
    public class ServiceOptions {
        public string Name { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string Host { get; set; } = null!;

        [JsonIgnore]
        public int PortParsed => int.Parse(Port);
    }
}
