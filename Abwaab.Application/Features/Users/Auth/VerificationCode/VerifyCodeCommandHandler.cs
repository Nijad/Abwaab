using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Common.Exceptions.Profile.VerificationCode;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Auth.Login;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Auth.VerificationCode
{
    public class VerifyCodeCommandHandler : IRequestHandler<VerifyCodeDTO, VerifyCodeResponse>
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationCodeService _verificationService;
        private readonly IProfileService _profileService;
        private readonly ILogger<VerifyCodeCommandHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly string errorTitle = ErrorTitle.VerificationCode;

        public VerifyCodeCommandHandler(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            IVerificationCodeService verificationService,
            IProfileService profileService,
            ILogger<VerifyCodeCommandHandler> logger,
            IJwtService jwtService)
        {
            _userService = userService;
            _userManager = userManager;
            _verificationService = verificationService;
            _profileService = profileService;
            _logger = logger;
            _jwtService = jwtService;
        }
        public async Task<VerifyCodeResponse> Handle(VerifyCodeDTO request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType, errorTitle);

            if(user == null)
                throw new UserNotFoundException(request.Identifier, errorTitle);

            bool isValid = await _verificationService.VerifyCodeAsync(request.Identifier, request.Code);

            if (!isValid)
                throw new InvalidVerificationCodeException(errorTitle);

            // Confirm the user's email or phone number based on the identifier type
            if (request.IdentifierType == IdentifiersEnum.Email)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                    throw new FailedConfirmationEmailException(errorTitle);

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWaysEnum.Email);
            }
            else if (request.IdentifierType == IdentifiersEnum.Phone_Number)
            {
                string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.Identifier);

                IdentityResult result = await _userManager.ChangePhoneNumberAsync(user, request.Identifier, token);

                if (!result.Succeeded)
                    throw new FailedConfirmationPhoneException(errorTitle);

                await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWaysEnum.SMS);
            }

            // Add the user to the "User" role
            if (! await _userManager.IsInRoleAsync(user, RoleConstants.ROLE_USER))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, RoleConstants.ROLE_USER);
                if (!roleResult.Succeeded)
                {
                    string errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));

                    _logger.LogError($"Failed to add user {user.Id} to 'User' role': {errors}");
                }
            }

            // Assign default plant to the new user
            await _userService.ActiveDefaultPlantAsync(user, errorTitle);

            // Subscribe the user to push notifications
            await _profileService.SubscribeNotificationWayCommandAsync(user, NotificationWaysEnum.Web_Application);


            //get user roles
            IList<string> roles = await _userManager.GetRolesAsync(user);

            // Generate access token
            TokenResponseDTO tokenResponse = await _jwtService.GenerateTokenResponseAsync(user, roles, request.Identifier);

            // Check if user has Admin role
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            VerifyCodeResponse response = new VerifyCodeResponse
            {
                Success = true,
                Message = "تم تأكيد الحساب وتسجيل دخولك بنجاح",
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                IsAdmin = isAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return response;
        }
    }
}
