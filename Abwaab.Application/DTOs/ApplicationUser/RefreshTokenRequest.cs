using MediatR;

namespace Abwaab.Application.DTOs.ApplicationUser
{
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
