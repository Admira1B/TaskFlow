using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Options {
    public class JsonWebTokenOptions {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public string Issuer { get; set; } = null!;

        [Required(ErrorMessage = "JWT Audience is required")]
        public string Audience { get; set; } = null!;

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public string SecretKey { get; set; } = null!;
    }

    [OptionsValidator]
    public partial class JsonWebTokenValidator : IValidateOptions<JsonWebTokenOptions> { }

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
