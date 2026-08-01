using Abwaab.Application.Common.Exceptions.Profile;
using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Password.Change
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordDTO, ChangePasswordResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(
            IProfileService profileService, IUserContext userContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ISmsSender smsSender, ILogger<ChangePasswordCommandHandler> logger)
        {
            _profileService = profileService;
            _userContext = userContext;
            _userManager = userManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = logger;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ChangePasswordResponse { Success = false, Message = "User not found." };

            // 1. Change the password
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                
                _logger.LogError($"Failed to change password for user {userId}. Errors: {errors}");

                throw new FailedChangePasswordException();
            }

            // null = unlock immediately
            await _userManager.SetLockoutEndDateAsync(user, null);
            // Reset failed attempts to 0
            await _userManager.ResetAccessFailedCountAsync(user);


            await _profileService.ChangePasswordCommandAsync(userId);

            if (user.Email != null)
                _ = Task.Run(async () =>
                {
                    var subject = "Security Alert: Your Password Was Changed";
                    var body = $@"
                    <h2>Password Changed</h2>
                    <p>Your account password was recently changed.</p>
                    <p><strong>Date/Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                    <p><strong>IP Address:</strong> {_userContext.RemoteIpAddress}</p>
                    <p>If you did NOT make this change, please reset your password immediately.</p>
                ";
                    await _emailSender.SendEmailAsync(user.Email, subject, body);
                });

            if (user.PhoneNumber != null)
                _ = Task.Run(async () =>
                {
                    var message = $"Your account password was recently changed. If you did NOT make this change, please reset your password immediately.";
                    await _smsSender.SendSmsAsync(user.PhoneNumber, message);
                });

            return new ChangePasswordResponse { Success = true, Message = "Password changed successfully. You have been logged out of all other devices." };
        }
    }
}
