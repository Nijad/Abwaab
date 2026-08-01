using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Profile;
using Abwaab.Application.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Password.Forgot
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordDTO, ForgotPasswordResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(
            IProfileService profileService, 
            IUserService userService,
            UserManager<ApplicationUser> userManager, 
            ILogger<ForgotPasswordCommandHandler> logger)
        {
            _profileService = profileService;
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordDTO request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindUserByIdentifierAsync(request.Identifier, request.IdentifierType);
            if (user == null)
                throw new NotFoundException("User", request.IdentifierType.ToString().Replace('_', ' '), request.Identifier);

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                _logger.LogError("Password reset failed for user {UserId}. Errors: {Errors}", user.Id, errors);

                throw new FailedChangePasswordException();
            }

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);

            await _profileService.RevokeAllRefreshToken(user.Id, "ForgotPassword");

            return new ForgotPasswordResponse { Success = true, Message = "Password reset successful." };
        }
    }
}
