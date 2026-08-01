using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Abwaab.Application.Features.Users.Auth.VerificationCode
{
    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeDTO, VerifyCodeResponse>
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationCodeService _verificationService;
        private readonly IProfileService _profileService;
        public VerifyCodeCommandHandler(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            IVerificationCodeService verificationService,
            IProfileService profileService)
        {
            _userService = userService;
            _userManager = userManager;
            _verificationService = verificationService;
            _profileService = profileService;
        }
        public async Task<VerifyCodeResponse> Handle(VerifyCodeDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);

            if(user == null)
                throw new NotFoundException("User", nameof(request.Identifier), request.Identifier);

            bool isValid = await _verificationService.VerifyCodeAsync(request.Identifier, request.Code);

            if (!isValid)
                throw new InvalidVerificationCodeException();

            if (request.IdentifierType == IdentifierEnum.email)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                    throw new FailedConfirmationEmailException();

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.Email);
            }
            else if (request.IdentifierType == IdentifierEnum.phone_number)
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.Identifier);

                var result = await _userManager.ChangePhoneNumberAsync(user, request.Identifier, token);

                if (!result.Succeeded)
                    throw new FailedConfirmationPhoneException();

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.SMS);
            }

            await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWayEnum.Push_Notification);

            return await Task.FromResult(new VerifyCodeResponse { Success = true, Message = "Verification successful." });
        }
    }
}
