using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
