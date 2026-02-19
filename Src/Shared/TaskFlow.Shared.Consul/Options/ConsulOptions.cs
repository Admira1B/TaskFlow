using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Consul.Options {
    public class ConsulOptions(string Datacenter, string Host, int Port, bool EnableServiceDiscovery) {
        [Required(ErrorMessage = "Consul datacenter is required")]
        public required string Datacenter { get; init; } = Datacenter;

        [Required(ErrorMessage = "Consul host is required")]
        public required string Host { get; init; } = Host;

        [Required(ErrorMessage = "Consul port is required")]
        [Range(1, 65535, ErrorMessage = "Consul port must be between 1 and 65535")]
        public int Port { get; init; } = Port;

        public required bool EnableServiceDiscovery { get; init; } = EnableServiceDiscovery;

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ConsulOptionsValidator : IValidateOptions<ConsulOptions> { 
    }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddConsulOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<ConsulOptions>()
                .Bind(configuration.GetSection(nameof(ConsulOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
