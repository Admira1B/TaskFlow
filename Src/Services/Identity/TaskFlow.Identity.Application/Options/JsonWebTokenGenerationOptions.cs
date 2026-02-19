using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Identity.Application.Options {
    public class JsonWebTokenGenerationOptions(string Issuer, string SecretKey, string[] ValidAudiences, int ExpiresHours) {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public required string Issuer { get; init; } = Issuer;

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public required string SecretKey { get; init; } = SecretKey;

        [Required(ErrorMessage = "At least one valid audience is required")]
        [MinLength(1, ErrorMessage = "ValidAudiences must contain at least one audience")]
        public required string[] ValidAudiences { get; init; } = ValidAudiences;

        [Required(ErrorMessage = "JWT ExpiresHours is required")]
        [Range(1, 240, ErrorMessage = "ExpiresHours must be between 1 and 240 (10 days)")]
        public int ExpiresHours { get; init; } = ExpiresHours;
    }

    [OptionsValidator]
    public partial class JsonWebTokenGenerationValidator : IValidateOptions<JsonWebTokenGenerationOptions> { 
    }

    public static partial class OptionsExtensions {
        public static IServiceCollection AddJsonWebTokenGenerationOptions(this IServiceCollection services, IConfiguration configuration) {
            services.AddOptions<JsonWebTokenGenerationOptions>()
                .Bind(configuration.GetSection(nameof(JsonWebTokenGenerationOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
