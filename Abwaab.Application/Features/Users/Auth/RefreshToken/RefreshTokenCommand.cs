using MediatR;

namespace Abwaab.Application.Features.Users.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
