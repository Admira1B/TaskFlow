using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TaskFlow.Shared.Core.Options {
    public class JsonWebTokenOptions {
        [Required(ErrorMessage = "JWT Issuer is required")]
        public required string Issuer { get; init; }

        [Required(ErrorMessage = "JWT Audience is required")]
        public required string Audience { get; init; }

        [Required(ErrorMessage = "JWT SecretKey is required")]
        public required string SecretKey { get; init; }
    }

    [OptionsValidator]
    public partial class JsonWebTokenOptionsValidator : IValidateOptions<JsonWebTokenOptions> {
    }
}
