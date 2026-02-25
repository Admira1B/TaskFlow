using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Messaging.Options {
    public class RabbitMqOptions {
        [Required(ErrorMessage = "RabbitMQ username is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "RabbitMQ password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "RabbitMQ virtual host is required")]
        public string VirtualHost { get; set; } = null!;

        [Required(ErrorMessage = "RabbitMQ host is required")]
        public string Host { get; set; } = null!;

        [Required(ErrorMessage = "RabbitMQ port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; set; }
    }

    [OptionsValidator]
    public partial class RabbitMqOptionsValidator : IValidateOptions<RabbitMqOptions> { }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddRabbitMqOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<RabbitMqOptions>()
                .Bind(configuration.GetSection(nameof(RabbitMqOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
