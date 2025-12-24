using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Enums;

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
                ErrorEnum.EntityNotFound => new NotFoundObjectResult(result.ErrorMessage),
                ErrorEnum.ValidationFailed => new BadRequestObjectResult(result.ErrorMessage),
                ErrorEnum.AlreadyExists => new ConflictObjectResult(result.ErrorMessage),
                ErrorEnum.InvalidOperation => new BadRequestObjectResult(result.ErrorMessage),
                _ => new BadRequestObjectResult("An unexpected error occurred")
            };
        }
    }
}
