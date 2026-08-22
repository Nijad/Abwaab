using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Domain.Entities.UserEntities;
using DynamicData;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;
using Whipstaff.Core.Entities;

namespace Abwaab.Application.Features.Users.Profile.Queries.UserProfileData
{
    public class UserProfileQueryHandler : IRequestHandler<UserProfileDataDTO, UserProfileDataResponse>
    {
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;
        private readonly IMemoryCache _cache;

        private readonly string errorTitle = ErrorTitle.ProfileData;

        public UserProfileQueryHandler(
            IUserService userService,
            IProfileService profileService,
            IMemoryCache cache)
        {
            _userService = userService;
            _profileService = profileService;
            _cache = cache;
        }

        public async Task<UserProfileDataResponse> Handle(UserProfileDataDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if (user == null)
                throw new Exception();
            bool hasActivatedEmailWay = await _profileService.HasActivatedEmailNotificationWay(user.Id, errorTitle);
            bool hasActivatedSmsWay = await _profileService.HasActivatedSmsNotificationWay(user.Id, errorTitle);

            List<PendingChangeIdentifierDTO> pcis = new();
            var cacheKey = $"email_change_{user.Id}";
            if (_cache.TryGetValue(cacheKey, out PendingEmailChange pendingEmail))
                pcis.Add(new()
                {
                    IdentifierType = "Email",
                    Identifier = pendingEmail.NewEmail,
                    CancelCode = pendingEmail.CancelCode
                });

            if (_cache.TryGetValue(cacheKey, out PendingPhoneChange pendingPhone))
                pcis.Add(new()
                {
                    IdentifierType = "PhoneNo",
                    Identifier = pendingPhone.NewPhoneNo,
                    CancelCode = pendingPhone.CancelCode
                });

            UserProfileDataResponse response = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Identifier = request.Identifier,
                Email = user.Email,
                EmailIsVerified = user.EmailConfirmed,
                MobileNumber = user.PhoneNumber,
                MobileIsVerified = user.PhoneNumberConfirmed,
                EmailNotificationStatus = hasActivatedEmailWay,
                SmsNotificationStatus = hasActivatedSmsWay,
                PendingChanges = pcis
            };

            return response;
        }
    }
}
