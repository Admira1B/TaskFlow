using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Messaging.Options {
    public class RabbitMqOptions(string UserName, string Password, string VirtualHost, string Host, int Port) {
        [Required(ErrorMessage = "RabbitMQ username is required")]
        public required string UserName { get; init; } = UserName;

        [Required(ErrorMessage = "RabbitMQ password is required")]
        public required string Password { get; init; } = Password;

        [Required(ErrorMessage = "RabbitMQ virtual host is required")]
        public required string VirtualHost { get; init; } = VirtualHost;

        [Required(ErrorMessage = "RabbitMQ host is required")]
        public required string Host { get; init; } = Host;
        
        [Required(ErrorMessage = "RabbitMQ port is required")]
        [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
        public int Port { get; init; } = Port;
    }

    [OptionsValidator]
    public partial class RabbitMqOptionsValidator : IValidateOptions<RabbitMqOptions> { 
    }

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
