using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Features.Users.Auth.RefreshToken;
using Abwaab.Application.Interfaces;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(
            IOptions<JwtSettings> settings,
            IRefreshTokenRepository refreshTokenRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = settings.Value;
            _refreshTokenRepo = refreshTokenRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateAccessToken(ApplicationUser user, IList<string> roles, string loginIdentifier)
        {
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Id.ToString()),
                new Claim("LoginIdentifier", loginIdentifier)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<Guid> GetUserIdByTokenAsync(RefreshTokenCommand request, string errorTitle)
        {
            RefreshToken? storedToken = await _refreshTokenRepo.GetByTokenAsync(request.RefreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
                throw new InvalidRefreshTokenException(errorTitle);

            return storedToken.UserId;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(ApplicationUser user, IList<string> roles, string refreshToken, string loginIdentifier)
        {
            await _refreshTokenRepo.RevokeAsync(refreshToken, "Rotation");
            var newAccessToken = GenerateAccessToken(user, roles, loginIdentifier);
            var newRefreshToken = GenerateRefreshToken();
            var newStoredToken = new RefreshToken
            {
                TokenHash = HashToken(newRefreshToken),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.CreateAsync(newStoredToken);

            return new RefreshTokenResponse
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = _jwtSettings.AccessTokenExpiryMinutes * 60
            };
        }

        private string HashToken(string token) // same as above
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        public async Task<TokenResponseDTO> GenerateTokenResponseAsync(ApplicationUser user, IList<string> roles, string loginIdentifier)
        {
            string accessToken = GenerateAccessToken(user, roles, loginIdentifier);
            string refreshTokenString = GenerateRefreshToken();
            string tokenHash = HashToken(refreshTokenString);
            var refreshToken = new RefreshToken
            {
                TokenHash = tokenHash,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.CreateAsync(refreshToken);

            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,           // Not accessible via JavaScript
                Secure = true,             // Only sent over HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ExpiryDate
            };

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append("RefreshToken", refreshTokenString, cookieOptions);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
                ExpiresIn = _jwtSettings.AccessTokenExpiryMinutes * 60,
            };
        }

    }
}