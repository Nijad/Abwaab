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

            string changingCode = Guid.NewGuid().ToString();
            var cancelUrl = _urlBuilder.GetCancelPhoneChangeUrl(changingCode);
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                _ = Task.Run(async () =>
                {
                    var alertMessage = $"تنبيه أمني: رقم هاتفك يجري  تعديل الآن {request.NewPhoneNo}. إذا لم تكن أنت يرجى إلغاء العملية حالاً {cancelUrl}";

                    await _smsSender.SendSmsAsync(user.PhoneNumber, alertMessage, errorTitle);
                });

            if (!string.IsNullOrEmpty(user.Email))
                _ = Task.Run(async () =>
                {
                    var alertSubject = "تنبيه أمني: طلب تغيير رقم الموبايل";
                    var alertBody = $@"
                        <h2>تنبيه أمني: طلب تغيير  رقم الموبايل</h2>
                        <p>لقد تلقينا طلب بتغيير رقم الموبايل المرتبط بحسابك.</p>
                        <p><strong>رقم الموبايل الجديد:</strong> {request.NewPhoneNo}</p>
                        <p><strong>الوقت:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        <p>إذا كنت أنت من قام بهذا الطلب, يرجى إدخال رمز التحقق المرسل إلى الرقم الجديد.</p>
                        <p><strong>إذا لم تكن أنت من قام بهذا الطلب, اضغط على الرباط أدناه لإلغاء عملية التغيير:</strong></p>
                        <p><a href='{cancelUrl}'>إلغاء تغيير رقم الموبايل</a></p>
                        <p>هذا الإجراء سيؤدي إلى تسجيل خروجك من جميع الأجهزة لدواعي أمنية.</p>
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
                UserId  = userId,
                NewPhoneNo = request.NewPhoneNo,
                OldPhoneNo = user.PhoneNumber,
                OldEmail = user.Email,
                Code = code,
                CreatedAt = DateTime.UtcNow
            };

            _cache.Set($"email_change_{userId}", pending, TimeSpan.FromMinutes(GeneralConstants.CODE_TIMEOUT_MINUTES));

            _cache.Set($"phone_change_{changingCode}", pending, TimeSpan.FromMinutes(5));

            return new InitiatePhoneNoChangeResponse { Success = true, Message = "تم إرسال رمز تحقق إلى رقم الموبايل الجديد." };
        }
    }
}
