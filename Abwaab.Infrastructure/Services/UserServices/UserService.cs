using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Application.Repositories;
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
        private readonly IPlanRepository _PlanRepository;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepo, IPlanRepository planRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepo = refreshTokenRepo;
            _PlanRepository = planRepository;
        }

        public string? FindUserByContext()
        {
            HttpContext? context = _httpContextAccessor.HttpContext;
            var username = context?.User?.Identity?.Name;
            return username;
        }

        public async Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifiersEnum identifierType)
        {
            ApplicationUser? user = null;
            if (identifierType == IdentifiersEnum.Email)
            {
                user = await _userManager.FindByEmailAsync(identifier);
                if (user != null)
                    return user;
                return await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PreviousEmail == identifier);
            }
            else if (identifierType == IdentifiersEnum.Phone_Number)
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

        public async Task AssignDefaultPlantAsync(Guid userId)
        {
            // Assign default plan to the user if they don't have an active plan
            Plan? defaultPlan = await _PlanRepository.GetDefaultPlanAsync();

            bool userHasActivePlan = await _PlanRepository.UserHasActivePlanAsync(userId);

            if (defaultPlan != null && !userHasActivePlan)
                await _PlanRepository.AssignPlanToUserAsync(userId, defaultPlan.Id);
        }
    }
}
