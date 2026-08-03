using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepo)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public string? FindUserByContext()
        {
            HttpContext? context = _httpContextAccessor.HttpContext;
            var username = context?.User?.Identity?.Name;
            return username;
        }

        public async Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifierEnum identifierType)
        {
            ApplicationUser? user = null;
            if (identifierType == IdentifierEnum.email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousEmail == identifier);
            }
            else if (identifierType == IdentifierEnum.phone_number)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousPhoneNumber == identifier);
            }

            throw new NotImplementedIdentifierException(identifierType.ToString());
        }

        public string? GetUserJti()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? jti = httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
            return jti;
        }

        public string? GetUserExpClaim()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? expClaim = httpContext?.User.FindFirst("exp")?.Value;
            return expClaim;
        }

        public void RemoveCookie(string cookieName)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
                httpContext.Response.Cookies.Delete(cookieName);
        }

        public async Task<LogoutResponse> RevokeActiveToken(Guid userId, bool revokeAll)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;

            if (revokeAll)
            {
                IEnumerable<RefreshToken> tokens = await _refreshTokenRepo.GetActiveTokensForUserAsync(userId);
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
