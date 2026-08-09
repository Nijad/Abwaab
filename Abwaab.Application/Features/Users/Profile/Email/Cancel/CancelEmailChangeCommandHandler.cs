using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeCommandHandler : IRequestHandler<CancelEmailChangeCommand, CancelEmailChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;
        public CancelEmailChangeCommandHandler(
            IProfileService profileService,
            IUserContext userContext,
            IMemoryCache cache)
        {
            _profileService = profileService;
            _userContext = userContext;
            _cache = cache;
        }
        public async Task<CancelEmailChangeResponse> Handle(CancelEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            // Check if there is a pending change
            var cacheKey = $"email_change_{userId}";
            if (!_cache.TryGetValue(cacheKey, out PendingEmailChange pending))
                throw new NoPendingEmailChangeException();

            // Remove the pending change from cache
            _cache.Remove(cacheKey);

            // Revoke ALL refresh tokens (force logout on all devices)
            await _profileService.RevokeAllRefreshToken(userId, "Cancelled by user");
            
            return new() { Success = true, Message = "Pending change cancelled. You have been logged out for security." };
        }
    }
}
