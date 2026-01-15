using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Identity.Contracts.DTOs.Requests.Role {
    public record GetRolesPaginatedRequest(
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        int Page = 1,

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        int PageSize = 10
    );
}
