using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ITokenCacheService _tokenCacheService;

        public LogoutCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ITokenCacheService tokenCacheService)
        {
            _userManager = userManager;
            _userService = userService;
            _tokenCacheService = tokenCacheService;
        }

        public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var jti = _userService.GetUserJti();

            if (!string.IsNullOrEmpty(jti))
            {
                string? expClaim = _userService.GetUserExpClaim();
                if (long.TryParse(expClaim, out var exp))
                {
                    DateTime expiry = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                    _tokenCacheService.AddToBlacklist(jti, expiry);
                }
            }

            // Remove the refresh token cookie
            _userService.RemoveCookie("RefreshToken");

            // get username from context
            string? username = _userService.FindUserNameByContext();

            ApplicationUser? user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new NotFoundException("User", nameof(username), username);

            LogoutResponse response = await _userService.RevokeActiveToken(user.Id, request.RevokeAll);

            return response;
        }
    }
}
