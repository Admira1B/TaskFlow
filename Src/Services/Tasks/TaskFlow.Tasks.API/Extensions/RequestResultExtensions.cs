using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.API.Extensions {
    public static class RequestResultExtensions {
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
                _ => new BadRequestObjectResult("An unexpected error occurred")
            };
        }
    }
}
