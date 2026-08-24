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
        private readonly string errorTitle = ErrorTitle.InitiateEmailChange;

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
                throw new UserNotFoundException(userId.ToString(), errorTitle);

            // ----------------------------------------------------------------
            // 1. SECURITY: Verify the user's current password (Critical!)
            // ----------------------------------------------------------------
            if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
                throw new InvalidCredentialsException(errorTitle);

            // Check if new email is the same as current
            if (user.Email?.Equals(request.NewEmail, StringComparison.OrdinalIgnoreCase) == true)
                throw new YourCurrentEmailException(errorTitle);

            // Check if new email is taken by another account
            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            if (existingUser != null && existingUser.Id != userId)
                throw new EmailAlreadyInUseException(errorTitle);

            // ----------------------------------------------------------------
            // 2. NOTIFY THE OLD CONTACT (Security Alert)
            //    Send an email to the OLD address to alert the user.
            //    This runs in the background (fire and forget) so we don't slow down the response.
            // ----------------------------------------------------------------
            Guid changingCode = Guid.NewGuid();
            var cancelUrl = _urlBuilder.GetCancelEmailChangeUrl(changingCode.ToString());
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"تنبيه أمني: بريدك الالكتروني يجري تعديله الآن to {request.NewEmail}. إذا لم تكن أنت يرجى إلغاء العملية حالاً من خلال الضغط على الرابط التالي: {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage, errorTitle);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "تنبيه أمني: طلب تغيير البريد الالكتروني";
                    var alertBody = $"<h2>تنبيه أمني: طلب تغيير البريد الالكتروني</h2>" +
                    $"<p>لقد تلقينا طلب تغيير البريد الالكتروني المرتبط بحسابك.</p>" +
                    $"<p><strong>البريد الالكتروني الجديد:</strong> {request.NewEmail}</p>" +
                    $"<p><strong>الوقت:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>" +
                    $"<p>إذا كنت أنت من قام بهذا الطلب، يرجى إدخال رمز التحقق المرسل إلى البريد الالكتروني الجديد.</p>" +
                    $"<p><strong>إذا لم تكن أنت من قام بهذا الطلب, اضغط على الرباط أدناه لإلغاء عملية التغيير:</strong></p>" +
                    $"<p>" +
                    $"<a href=\""+cancelUrl+"\">إلغاء تغيير البريد الالكتروني</a>" +
                    "</p>" +
                    "<p>هذا الإجراء سيؤدي إلى تسجيل خروجك من جميع الأجهزة لدواعي أمنية.</p>";
                    await _emailSender.SendEmailAsync(user.Email, alertSubject, alertBody, errorTitle);
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
                UserId = userId,
                NewEmail = request.NewEmail,
                OldEmail = user.Email,
                OldPhoneNo = user.PhoneNumber,
                Code = code,
                CreatedAt = DateTime.UtcNow,
                CancelCode = changingCode
            };

            _cache.Set($"email_change_{userId}", pending, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            _cache.Set($"email_change_{changingCode}", pending, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            // We don't mention the alert to the user to avoid confusion, but it's sent.
            return new InitiateEmailChangeResponse { Success = true, Message = "تم إرسال رمز تحقق إلى عنوان البريد الالكتروني الجديد." };
        }
    }
}
