using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Identity.Domain.Enums;
using TaskFlow.Identity.Domain.Options;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.API.Extensions {
    public static class ResultsExtensions {
        private const int _baseTokenLifetime = 8;

        public static IActionResult ToActionResult<T>(this RequestResult<T> result) {
            if (result.Succeeded) {
                return result.Value switch {
                    null => new NoContentResult(),
                    Unit => new NoContentResult(),
                    _ => new OkObjectResult(result.Value)
                };
            }

            return result.ErrorType switch {
                ErrorType.AlreadyExists => new ConflictObjectResult(result.ErrorMessage),
                ErrorType.EntityNotFound => new NotFoundObjectResult(result.ErrorMessage),
                ErrorType.ValidationFailed => new BadRequestObjectResult(result.ErrorMessage),
                ErrorType.InvalidOperation => new BadRequestObjectResult(result.ErrorMessage),
                ErrorType.FailedToPublishEvent => new ObjectResult(result.ErrorMessage) {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                },
                _ => new BadRequestObjectResult("An unexpected error occurred")
            };
        }

        public static IActionResult ToActionResult(this AuthResult result, JsonWebTokenGenerationOptions jwtOptions) {
            if (result.Succeeded) {
                return new OkObjectResult(
                    new {
                        user = result.User,
                        token = result.Token,
                        // Token Lifetime in seconds
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
