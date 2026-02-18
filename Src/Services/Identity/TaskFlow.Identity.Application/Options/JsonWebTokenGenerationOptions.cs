using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Application.Options {
    public class JsonWebTokenGenerationOptions {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public required string Issuer { get; init; }

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public required string SecretKey { get; init; }

        [Required(ErrorMessage = "At least one valid audience is required")]
        [MinLength(1, ErrorMessage = "ValidAudiences must contain at least one audience")]
        public required string[] ValidAudiences { get; init; }

        [Required(ErrorMessage = "JWT ExpiresHours is required")]
        [Range(1, 240, ErrorMessage = "ExpiresHours must be between 1 and 240 (10 days)")]
        public int ExpiresHours { get; init; }
    }

    [OptionsValidator]
    public partial class ServiceOptionsValidator : IValidateOptions<JsonWebTokenGenerationOptions> { 
    }
}
