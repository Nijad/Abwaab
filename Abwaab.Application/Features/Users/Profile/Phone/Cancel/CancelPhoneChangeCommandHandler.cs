using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Phone.Cancel
{
    public class CancelPhoneChangeCommandHandler : IRequestHandler<CancelPhoneChangeCommand, CancelPhoneChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;
        private readonly string errorTitle = ErrorTitle.CancelPhoneNoChange;

        public CancelPhoneChangeCommandHandler(
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
        public async Task<CancelPhoneChangeResponse> Handle(CancelPhoneChangeCommand request, CancellationToken cancellationToken)
        {
            //var userId = _userContext.UserId;
            var cacheCancelKey = $"phone_change_{request.ChangingCode}";
            

            if (!_cache.TryGetValue(cacheCancelKey, out PendingPhoneChange pending))
                throw new NoPendingPhoneChangeException(errorTitle);


            string oldEmail = pending.OldEmail;
            string oldPhoneNo = pending.OldPhoneNo;

            // Remove the pending change from cache
            _cache.Remove(cacheCancelKey);

            // Revoke ALL refresh tokens (force logout on all devices)
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(oldEmail, IdentifiersEnum.Email, errorTitle);
            if (user == null)
                user = await _userService.FindUserByIdentifierAsync(oldPhoneNo, IdentifiersEnum.Email, errorTitle);

            var cacheConfirmlKey = $"phone_change_{user.Id}";
            _cache.Remove(cacheConfirmlKey);

            await _profileService.RevokeAllRefreshToken(user.Id, "Cancelled by user");

            return new CancelPhoneChangeResponse { Success = true, Message = "تم إلغاء تعليق التغيير. You لقد تم تسجيل خروجك لدواعي أمنية." };
        }
    }
}
