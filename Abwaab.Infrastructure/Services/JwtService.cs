using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Abwaab.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly ILogger<JwtService> _logger;

        public JwtService(
            IOptions<JwtSettings> settings,
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepo,
            ILogger<JwtService> logger)
        {
            _jwtSettings = settings.Value;
            _userManager = userManager;
            _refreshTokenRepo = refreshTokenRepo;
            _logger = logger;
        }

        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Id.ToString())
            };

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

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            // 1. Validate the refresh token from the repository
            var storedToken = await _refreshTokenRepo.GetByTokenAsync(request.RefreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid or expired refresh token.");
                return new RefreshTokenResponse { Success = false, Message = "Invalid or expired refresh token." };
            }

            // 2. Find the user
            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User not found for refresh token.");
                return new RefreshTokenResponse { Success = false, Message = "User not found." };
            }

            // 3. (Optional) Revoke the old token (rotation)
            await _refreshTokenRepo.RevokeAsync(request.RefreshToken, "Rotation");

            // 4. Generate new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // 5. Store the new refresh token
            var newStoredToken = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            await _refreshTokenRepo.CreateAsync(newStoredToken);

            _logger.LogInformation("Refresh token rotated for user {UserId}.", user.Id);

            return new RefreshTokenResponse
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = (int)_jwtSettings.AccessTokenExpiryMinutes * 60
            };
        }
    }
}