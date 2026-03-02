using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Options {
    public class ServiceOptions {
        [Required(ErrorMessage = "Service name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Service host is required")]
        public string Host { get; set; } = null!;

        [Required(ErrorMessage = "Service port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; set; } = 0;

        [JsonIgnore]
        public string Address => $"http://{Host}:{Port}";
    }

    [OptionsValidator]
    public partial class ServiceOptionsValidator : IValidateOptions<ServiceOptions> { }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddServiceOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<ServiceOptions>()
                .Bind(configuration.GetSection(nameof(ServiceOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public static ServiceOptions GetServiceOptions(this IConfiguration configuration) {
            return configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()
                   ?? throw new InvalidOperationException($"{nameof(ServiceOptions)} not configured");
        }
    }
}
