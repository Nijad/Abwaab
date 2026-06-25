namespace Abwaab.Application.DTOs.ApplicationUser
{
    public record LoginUserResponse(bool IsSuccessful, string? Message = null, string? Token = null);
}
