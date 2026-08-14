using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Features.Users.Auth.RefreshToken;
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken();
        Task<TokenResponseDTO> GenerateTokenResponseAsync(ApplicationUser user, IList<string> roles);
        Task<RefreshTokenResponse> RefreshTokenAsync(ApplicationUser user, IList<string> roles, string refreshToken);

        Task<Guid> GetUserIdByTokenAsync(RefreshTokenCommand request, string errorTitle);
    }
}
