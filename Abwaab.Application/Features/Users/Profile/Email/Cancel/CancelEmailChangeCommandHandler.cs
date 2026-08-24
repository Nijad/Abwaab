using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Email.Cancel
{
    public class CancelEmailChangeCommandHandler : IRequestHandler<CancelEmailChangeCommand, CancelEmailChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.CancelEmailChange;

        public CancelEmailChangeCommandHandler(
            IProfileService profileService,
            IUserContext userContext,
            IMemoryCache cache,
            IUserService userService)
        {
            _profileService = profileService;
            _userContext = userContext;
            _cache = cache;
            _userService = userService;
        }
        public async Task<CancelEmailChangeResponse> Handle(CancelEmailChangeCommand request, CancellationToken cancellationToken)
        {
            //var userId = _userContext.UserId;

            // Check if there is a pending change
            var cacheCancelKey = $"email_change_{request.ChangingCode}";

            if (!_cache.TryGetValue(cacheCancelKey, out PendingEmailChange pending))
                throw new NoPendingEmailChangeException(errorTitle);

            string oldEmail = pending.OldEmail;
            string oldPhoneNo = pending.OldPhoneNo;

            // Remove the pending change from cache
            _cache.Remove(cacheCancelKey);

            // Revoke ALL refresh tokens (force logout on all devices)
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(oldEmail, IdentifiersEnum.Email, errorTitle);
            if (user == null)
                user = await _userService.FindUserByIdentifierAsync(oldPhoneNo, IdentifiersEnum.Email, errorTitle);

            var cacheConfirmlKey = $"email_change_{user.Id}";
            _cache.Remove(cacheConfirmlKey);

            await _profileService.RevokeAllRefreshToken(user.Id, "Cancelled by user");

            return new() { Success = true, Message = "تم إلغاء التغيير وتسجيل خروجك لدواعي أمنية." };
        }
    }
}
