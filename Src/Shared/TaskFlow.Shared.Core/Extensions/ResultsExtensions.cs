using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TaskFlow.Shared.Core.Enums;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Shared.Core.Extensions {
    public static class ResultsExtensions {
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
    }
}
