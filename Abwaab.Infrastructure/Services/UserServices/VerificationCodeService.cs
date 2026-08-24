using Abwaab.Application.Common.Constants;
using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;
        private readonly string errorTitle = ErrorTitle.VerificationCode;

        public VerificationCodeService(
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _cache = cache;
        }

        public string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<SendCodeResponse> SendVerificationCodeAsync(SendCodeDTO resendCodeDTO, string errorTitle)
        {
            if (resendCodeDTO.IdentifierType == IdentifiersEnum.Email)
            {
                // Send the code to the email
                return await SendVerificationCodeViaEmailAsync(resendCodeDTO.Identifier, resendCodeDTO.Code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "تم إرسال رمز التحقق إلى بريدك الالكتروني."
                    });
            }
            else if (resendCodeDTO.IdentifierType == IdentifiersEnum.Phone_Number)
            {
                // Send the code to the phone number
                return await SendVerificationCodeViaSmsAsync(resendCodeDTO.Identifier, resendCodeDTO.Code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "تم إرسال رمز التحقق إلى رقم هاتفك."
                    });
            }

            throw new NotImplementedIdentifierException(resendCodeDTO.IdentifierType.ToString(), errorTitle);
        }

        public async Task SendVerificationCodeViaEmailAsync(string email, string code)
        {
            var subject = "رمز التحقق الخاص بحسابك";
            var body = $@"
                <h2>التحقق من البريد الالكتروني</h2>
                <p>نشكرك على التسجيل. يرجى استخدام الرمز التالي للتحقق من حسابك:</p>
                <h1 style='font-size: 32px; letter-spacing: 5px; color: #2d3748;'>{code}</h1>
                <p>هذا الرمز صالح لمدة {GeneralConstants.CODE_TIMEOUT_MINUTES} دقائق.</p>
                <p>إذا لم تطلب ذلك، فيرجى تجاهل هذه الرسالة الإلكترونية.</p>
            ";

            await _emailSender.SendEmailAsync(email, subject, body, errorTitle);
        }

        public async Task SendVerificationCodeViaSmsAsync(string phoneNo, string code)
        {
            var message = $"رمز التحقق هذا: {code[0]}{code[1]}{code[2]}-{code[3]}{code[4]}{code[5]} (صالح لمدة {GeneralConstants.CODE_TIMEOUT_MINUTES} دقائق)";

            await _smsSender.SendSmsAsync(phoneNo, message, errorTitle);
        }

        public Task<bool> VerifyCodeAsync(string identifier, string userInputCode)
        {
            if (_cache.TryGetValue(identifier, out string? storedCode) && storedCode == userInputCode)
            {
                    _cache.Remove(identifier); // Invalidate code after successful use
                    return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}