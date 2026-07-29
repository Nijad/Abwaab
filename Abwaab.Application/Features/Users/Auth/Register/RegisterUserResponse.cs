namespace Abwaab.Application.Features.Users.Auth.Register
{
    public record RegisterUserResponse(bool IsSuccessful, string? Message = null);
}
