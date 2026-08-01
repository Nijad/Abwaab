using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenCacheService _tokenCacheService;

        public AuthService(
            IRefreshTokenRepository refreshTokenRepo,
            IHttpContextAccessor httpContextAccessor,
            ITokenCacheService tokenCacheService)
        {
            _refreshTokenRepo = refreshTokenRepo;
            _httpContextAccessor = httpContextAccessor;
            _tokenCacheService = tokenCacheService;
        }

        public async Task<LogoutResponse> LogoutCommandAsync(LogoutCommand request)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? jti = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (!string.IsNullOrEmpty(jti))
            {
                string? expClaim = httpContext.User.FindFirst("exp")?.Value;
                if (long.TryParse(expClaim, out var exp))
                {
                    DateTime expiry = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                    _tokenCacheService.AddToBlacklist(jti, expiry);
                }
            }

            // 2. Remove the refresh token cookie
            httpContext.Response.Cookies.Delete("RefreshToken");

            // 3. Revoke refresh tokens (all or the specific one)
            if (request.RevokeAll)
            {
                IEnumerable<RefreshToken> tokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(request.UserId);
                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.RevokedByIp = "Logout all";
                    await _refreshTokenRepo.UpdateAsync(token);
                }
            }
            else
            {
                // Revoke only the one from the cookie
                string? refreshToken = httpContext.Request.Cookies["RefreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                    await _refreshTokenRepo.RevokeAsync(refreshToken, "Logout");
            }

            return new LogoutResponse { Success = true, Message = "Logged out successfully." };
        }
    }
}
