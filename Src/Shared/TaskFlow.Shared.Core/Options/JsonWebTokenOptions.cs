using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Options {
    public class JsonWebTokenOptions(string Issuer, string Audience, string SecretKey) {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public required string Issuer { get; init; } = Issuer;

        [Required(ErrorMessage = "JWT Audience is required")]
        public required string Audience { get; init; } = Audience;

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public required string SecretKey { get; init; } = SecretKey;
    }

    [OptionsValidator]
    public partial class JsonWebTokenValidator : IValidateOptions<JsonWebTokenOptions> {
    }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddJsonWebTokenOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<JsonWebTokenOptions>()
                .Bind(configuration.GetSection(nameof(JsonWebTokenOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
