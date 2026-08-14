using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Common.Exceptions.Auth;
using Abwaab.Application.Common.Exceptions.Profile.Phone;
using Abwaab.Application.Features.Users.Profile.Phone.Pending;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Application.Features.Users.Profile.Phone.InitiateChange
{
    public class InitiatePhoneChangeCommandHandler : IRequestHandler<InitiatePhoneNoChangeCommand, InitiatePhoneNoChangeResponse>
    {
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlBuilder _urlBuilder;
        private readonly ISmsSender _smsSender;
        private readonly IEmailSender _emailSender;
        private readonly IVerificationCodeService _verificationService;
        private readonly ILogger<InitiatePhoneChangeCommandHandler> _logger;
        private readonly IMemoryCache _cache;
        private readonly string errorTitle = ErrorTitle.InitiatePhoneChange;

        public InitiatePhoneChangeCommandHandler(
            IUserContext userContext, 
            UserManager<ApplicationUser> userManager, 
            IUrlBuilder urlBuilder, 
            ISmsSender smsSender, 
            IEmailSender emailSender, 
            IVerificationCodeService verificationService, 
            ILogger<InitiatePhoneChangeCommandHandler> logger, 
            IMemoryCache cache)
        {
            _userContext = userContext;
            _userManager = userManager;
            _urlBuilder = urlBuilder;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _verificationService = verificationService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<InitiatePhoneNoChangeResponse> Handle(InitiatePhoneNoChangeCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserNotFoundException(userId.ToString(), errorTitle);

            // ----------------------------------------------------------------
            // 1. SECURITY: Verify the user's current password (Critical!)
            // ----------------------------------------------------------------
            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                throw new InvalidCredentialsException(errorTitle);

            // Check if new phone is the same as current
            if (user.PhoneNumber?.Equals(request.NewPhoneNo, StringComparison.OrdinalIgnoreCase) == true)
                throw new YourCurrentPhoneException(errorTitle);

            // Check if the new phone number is already taken by another user
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewPhoneNo);
            if (existingUser != null && existingUser.Id != userId)
                throw new PhoneAlreadyInUseException(errorTitle);

            // ----------------------------------------------------------------
            // 2. NOTIFY THE OLD CONTACT (Security Alert)
            //    Send an SMS to the OLD number to alert the user.
            //    Fire-and-forget (background) so we don't slow down the response.
            // ----------------------------------------------------------------

            var cancelUrl = _urlBuilder.GetCancelPhoneChangeUrl();
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"SECURITY ALERT: Your phone is being changed to {request.NewPhoneNo}. If this wasn't you, cancel at {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage, errorTitle);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "Security Alert: Phone Change Requested";
                    var alertBody = $@"
                        <h2>Security Alert: Phone Change Requested</h2>
                        <p>We received a request to change the phone number associated with your account.</p>
                        <p><strong>New phone requested:</strong> {request.NewPhoneNo}</p>
                        <p><strong>Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        <p>If you made this request, please enter the verification code sent to your new phone number.</p>
                        <p><strong>If you did NOT request this, click the link below to cancel the change immediately:</strong></p>
                        <p><a href='{cancelUrl}'>Cancel Phone Change</a></p>
                        <p>This link will revoke all your active sessions for security.</p>
                    ";
                    await _emailSender.SendEmailAsync(user.Email, alertSubject, alertBody, errorTitle);
                });

            // ----------------------------------------------------------------
            // 3. VERIFY THE NEW CONTACT
            //    Send a verification code to the NEW phone number.
            // ----------------------------------------------------------------
            string code = _verificationService.GenerateVerificationCode();
            await _verificationService.SendVerificationCodeViaSmsAsync(request.NewPhoneNo, code);

            // Store the pending change in cache (valid for 5 minutes)
            var pending = new PendingPhoneChange
            {
                NewPhoneNo = request.NewPhoneNo,
                Code = code,
                CreatedAt = DateTime.UtcNow
            };
            _cache.Set($"phone_change_{userId}", pending, TimeSpan.FromMinutes(5));

            return new InitiatePhoneNoChangeResponse { Success = true, Message = "Verification code sent to the new phone number." };
        }
    }
}
