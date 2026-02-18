using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TaskFlow.Shared.Consul.Options {
    public class ConsulOptions {
        [Required(ErrorMessage = "Consul datacenter is required")]
        public required string Datacenter { get; init; }

        [Required(ErrorMessage = "Consul host is required")]
        public required string Host { get; init; }

        [Required(ErrorMessage = "Consul port is required")]
        [Range(1, 65535, ErrorMessage = "Consul port must be between 1 and 65535")]
        public int Port { get; init; }

        public required bool EnableServiceDiscovery { get; init; } = true;

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ServiceOptionsValidator : IValidateOptions<ConsulOptions> { 
    }
}
