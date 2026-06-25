namespace Abwaab.Application.DTOs.ApplicationUser
{
    public record RegisterUserResponse(bool IsSuccessful, string? Message = null);
}
