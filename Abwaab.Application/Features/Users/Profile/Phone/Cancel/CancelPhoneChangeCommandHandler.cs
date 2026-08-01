using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Phone.Cancel
{
    public class CancelPhoneChangeCommandHandler : IRequestHandler<CancelPhoneChangeCommand, CancelPhoneChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;

        public CancelPhoneChangeCommandHandler(
            IProfileService profileService, IUserContext userContext, IMemoryCache cache)
        {
            _profileService = profileService;
            _userContext = userContext;
            _cache = cache;
        }
        public async Task<CancelPhoneChangeResponse> Handle(CancelPhoneChangeCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var cacheKey = $"phone_change_{userId}";

            if (!_cache.TryGetValue(cacheKey, out PendingPhoneChange pending))
                throw new NoPendingPhoneChangeException();

            _cache.Remove(cacheKey);

            await _profileService.RevokeAllRefreshToken(userId, "Cancelled by user");
            
            return new CancelPhoneChangeResponse { Success = true, Message = "Pending change cancelled. You have been logged out for security." };
        }
    }
}
