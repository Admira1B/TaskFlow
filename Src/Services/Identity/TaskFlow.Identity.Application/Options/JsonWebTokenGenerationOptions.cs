using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Identity.Application.Options {
    public class JsonWebTokenGenerationOptions {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public string Issuer { get; set; } = null!;

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public string SecretKey { get; set; } = null!;

        [Required(ErrorMessage = "At least one valid audience is required")]
        [MinLength(1, ErrorMessage = "ValidAudiences must contain at least one audience")]
        public string[] ValidAudiences { get; set; } = null!;

        [Required(ErrorMessage = "JWT ExpiresHours is required")]
        [Range(1, 240, ErrorMessage = "ExpiresHours must be between 1 and 240 (10 days)")]
        public int ExpiresHours { get; set; }
    }

    [OptionsValidator]
    public partial class JsonWebTokenGenerationValidator : IValidateOptions<JsonWebTokenGenerationOptions> { }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddJsonWebTokenGenerationOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<JsonWebTokenGenerationOptions>()
                .Bind(configuration.GetSection(nameof(JsonWebTokenGenerationOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        public static JsonWebTokenGenerationOptions GetJsonWebTokenGenerationOptions(this IConfiguration configuration) {
            return configuration.GetSection(nameof(JsonWebTokenGenerationOptions)).Get<JsonWebTokenGenerationOptions>()
                   ?? throw new InvalidOperationException($"{nameof(JsonWebTokenGenerationOptions)} not configured");
        }
    }
}
