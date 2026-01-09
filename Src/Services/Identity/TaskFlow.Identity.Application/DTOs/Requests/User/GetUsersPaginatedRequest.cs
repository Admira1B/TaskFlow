using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Application.DTOs.Requests.User {
    public record GetUsersPaginatedRequest(
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        int Page = 1,

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        int PageSize = 20
    );
}
