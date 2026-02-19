using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Options {
    public class ServiceOptions(string Name, string Host, int Port) {
        [Required(ErrorMessage = "Service name is required")]
        public required string Name { get; init; } = Name;

        [Required(ErrorMessage = "Service host is required")]
        public required string Host { get; init; } = Host;

        [Required(ErrorMessage = "Service port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; init; } = Port;

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ServiceOptionsValidator : IValidateOptions<ServiceOptions>{
    }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddServiceOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<ServiceOptions>()
                .Bind(configuration.GetSection(nameof(ServiceOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
