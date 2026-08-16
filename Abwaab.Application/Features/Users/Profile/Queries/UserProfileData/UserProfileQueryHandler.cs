using Abwaab.Application.Common.Constants;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;

namespace Abwaab.Application.Features.Users.Profile.Queries.UserProfileData
{
    public class UserProfileQueryHandler : IRequestHandler<UserProfileDataDTO, UserProfileDataResponse>
    {
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;

        private readonly string errorTitle = ErrorTitle.ProfileData;

        public UserProfileQueryHandler(
            IUserService userService, 
            IProfileService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

        public async Task<UserProfileDataResponse> Handle(UserProfileDataDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if (user == null)
                throw new Exception();

            bool hasActivatedEmailWay = await _profileService.HasActivatedEmailNotificationWay(user.Id, errorTitle);
            bool hasActivatedSmsWay = await _profileService.HasActivatedSmsNotificationWay(user.Id, errorTitle);
            bool hasActivatedWebWay = await _profileService.HasActivatedWebNotificationWay(user.Id, errorTitle);

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
                WebAppNotificationStatus = hasActivatedWebWay,
                PasswordLastModified = "Not Implemented",
                PendingChanges = "Not Implemented"
            };

            return response;
        }
    }
}
