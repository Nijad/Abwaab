using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Email;
using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Users.Profile.Email.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Application.Features.Users.Profile.Email.InitiateChange
{
    public class InitiateEmailChangeCommandHandler : IRequestHandler<InitiateEmailChangeCommand, InitiateEmailChangeResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlBuilder _urlBuilder;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IVerificationCodeService _verificationService;
        private readonly IMemoryCache _cache;

        public InitiateEmailChangeCommandHandler(
            IProfileService profileService, 
            IUserContext userContext, 
            UserManager<ApplicationUser> userManager, 
            IUrlBuilder urlBuilder, 
            ISmsSender smsSender, 
            IEmailSender emailSender, 
            IVerificationCodeService verificationService, 
            IMemoryCache cache)
        {
            _profileService = profileService;
            _userContext = userContext;
            _userManager = userManager;
            _urlBuilder = urlBuilder;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _verificationService = verificationService;
            _cache = cache;
        }

        public async Task<InitiateEmailChangeResponse> Handle(InitiateEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("User", nameof(userId), userId.ToString());

            // ----------------------------------------------------------------
            // 1. SECURITY: Verify the user's current password (Critical!)
            // ----------------------------------------------------------------
            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                throw new InvalidCredentialsException();

            // Check if new email is the same as current
            if (user.Email?.Equals(request.NewEmail, StringComparison.OrdinalIgnoreCase) == true)
                throw new YourCurrentEmailException();

            // Check if new email is taken by another account
            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            if (existingUser != null && existingUser.Id != userId)
                throw new EmailAlreadyInUseException();

            // ----------------------------------------------------------------
            // 2. NOTIFY THE OLD CONTACT (Security Alert)
            //    Send an email to the OLD address to alert the user.
            //    This runs in the background (fire and forget) so we don't slow down the response.
            // ----------------------------------------------------------------
            var cancelUrl = _urlBuilder.GetCancelEmailChangeUrl();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"SECURITY ALERT: Your email is being changed to {request.NewEmail}. If this wasn't you, cancel at {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "Security Alert: Email Change Requested";
                    var alertBody = $@"
                        <h2>Security Alert: Email Change Requested</h2>
                        <p>We received a request to change the email address associated with your account.</p>
                        <p><strong>New email requested:</strong> {request.NewEmail}</p>
                        <p><strong>Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        <p>If you made this request, please enter the verification code sent to your new email.</p>
                        <p><strong>If you did NOT request this, click the link below to cancel the change immediately:</strong></p>
                        <p><a href='{cancelUrl}'>Cancel Email Change</a></p>
                        <p>This link will revoke all your active sessions for security.</p>
                    ";
                    await _emailSender.SendEmailAsync(user.Email, alertSubject, alertBody);
                });

            // ----------------------------------------------------------------
            // 3. VERIFY THE NEW CONTACT
            //    Send a verification code to the NEW email.
            // ----------------------------------------------------------------
            var code = _verificationService.GenerateVerificationCode();
            await _verificationService.SendVerificationCodeViaEmailAsync(request.NewEmail, code);

            // Store pending change
            var pending = new PendingEmailChange
            {
                NewEmail = request.NewEmail,
                Code = code,
                CreatedAt = DateTime.UtcNow
            };

            _cache.Set($"email_change_{userId}", pending, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            // We don't mention the alert to the user to avoid confusion, but it's sent.
            return new InitiateEmailChangeResponse { Success = true, Message = "Verification code sent to the new email address." };
        }
    }
}
