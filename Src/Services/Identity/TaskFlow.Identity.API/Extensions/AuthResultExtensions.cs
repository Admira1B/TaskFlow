using Microsoft.AspNetCore.Mvc;
using TaskFlow.Shared.Core.Enums;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Application.Options;

namespace TaskFlow.Identity.API.Extensions {
    public static class AuthResultExtensions {
        private const int _baseTokenLifetime = 8;
        public static IActionResult ToActionResult(this AuthResult result, JsonWebTokenGenerationOptions jwtOptions) {
            if (result.Succeeded) {
                return new OkObjectResult(
                    new {
                        user = result.User,
                        token = result.Token,
                        // 3600 is the number of seconds in 1h
                        expiresIn = (jwtOptions?.ExpiresHours ?? _baseTokenLifetime) * 3600
                    }
                );
            }

            return result.ErrorType switch {
                ErrorType.AlreadyExists => new ConflictObjectResult(result.ErrorMessage),
                ErrorType.ValidationFailed => new BadRequestObjectResult(result.ErrorMessage),
                ErrorType.EntityNotFound => new UnauthorizedObjectResult(result.ErrorMessage),
                ErrorType.InvalidCredentials => new UnauthorizedObjectResult(result.ErrorMessage),
                _ => new BadRequestObjectResult("An unexpected error occurred")
            };
        }
    }
}
