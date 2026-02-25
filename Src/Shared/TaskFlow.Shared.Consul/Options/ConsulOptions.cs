using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Consul.Options {
    public class ConsulOptions {
        [Required(ErrorMessage = "Consul datacenter is required")]
        public string Datacenter { get; set; } = null!;

        [Required(ErrorMessage = "Consul host is required")]
        public string Host { get; set; } = null!;

        [Required(ErrorMessage = "Consul port is required")]
        [Range(1, 65535, ErrorMessage = "Consul port must be between 1 and 65535")]
        public int Port { get; set; }

        public bool EnableServiceDiscovery { get; set; }

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ConsulOptionsValidator : IValidateOptions<ConsulOptions> { }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddConsulOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<ConsulOptions>()
                .Bind(configuration.GetSection(nameof(ConsulOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public static ConsulOptions GetConsulOptions(this IConfiguration configuration) {
            return configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>()
                   ?? throw new InvalidOperationException($"{nameof(ConsulOptions)} not configured");
        }
    }
}
