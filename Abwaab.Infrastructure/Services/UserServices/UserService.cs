using Abwaab.Application.Common.Enums;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.Plans;
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
        private readonly IPlanRepository _planRepository;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IRefreshTokenRepository refreshTokenRepo, IPlanRepository planRepository)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepo = refreshTokenRepo;
            _planRepository = planRepository;
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

        public async Task ActiveDefaultPlantAsync(ApplicationUser user)
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            string? actionUser = httpContext?.User?.Identity?.Name;

            // 1. check if user doesn't have active plan
            bool hasActivePlan = await _planRepository.CheckIfUserHasActivePlan(user.Id);

            // if user already has active plan return and don't throw exception
            // because if user add or change thier identifier
            if (hasActivePlan)
                return; // throw new UserAlreadyHasActivePlanException();

            // 2. check if user already has default plan 
            Plan? defaultPlan = await _planRepository.GetDefaultPlanAsync();

            if (defaultPlan == null)
                throw new NotFoundException(nameof(Plan), nameof(defaultPlan.DefaultPlan), "True");

            bool hasDefultPlan = await _planRepository.UserHasPlan(user.Id, defaultPlan.Id);

            // 3. active default plan if exists or create new one for user
            if (hasDefultPlan)
            {
                // active defult plan for user
                await _planRepository.ActiveUserPlan(user.Id, defaultPlan.Id);
            }
            else
            {
                // assign defaul plan to user
                Guid activeUserPlanStateId = await _planRepository.GetUserPlanStateId(UserPlanStatesEnum.Active);

                await _planRepository.AssignPlanToUserAsync(user.Id, defaultPlan.Id, activeUserPlanStateId);
            }                
        }

        public async Task<ApplicationUser?> FindUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}
