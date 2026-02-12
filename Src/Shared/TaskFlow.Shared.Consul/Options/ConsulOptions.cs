using System.Text.Json.Serialization;

namespace TaskFlow.Shared.Consul.Options {
    public class ConsulOptions {
        public string Address { get; set; } = null!;
        public string Datacenter { get; set; } = null!;
        public string EnableServiceDiscovery { get; set; } = null!;

        [JsonIgnore]
        public bool EnableServiceDiscoveryParsed => bool.Parse(EnableServiceDiscovery);
    }
}
