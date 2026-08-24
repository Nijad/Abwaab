using Abwaab.Application.Common.Exceptions;
using Abwaab.Application.Features.Users.Auth.SendCode;
using Abwaab.Application.Interfaces;
using Abwaab.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class VerificationCodeDemoService : IVerificationCodeService
    {
        private readonly IMemoryCache _cache;

        public VerificationCodeDemoService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GenerateVerificationCode()
        {
            return "123456";
        }

        public Task<SendCodeResponse> SendVerificationCodeAsync(SendCodeDTO resendCodeDTO, string errorTitle)
        {
            string code = GenerateVerificationCode();
            if (resendCodeDTO.IdentifierType == IdentifiersEnum.Email)
            {
                return SendVerificationCodeViaEmailAsync(resendCodeDTO.Identifier, code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "تم إرسال رمز التفعيل إلى بريدك الالكتروني."
                    });
            }
            else if (resendCodeDTO.IdentifierType == IdentifiersEnum.Phone_Number)
            {
                return SendVerificationCodeViaSmsAsync(resendCodeDTO.Identifier, code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "تم إرسال رمز التفعيل إلى رقم هاتفك."
                    });
            }
            else
            {
                throw new NotImplementedIdentifierException(resendCodeDTO.IdentifierType.ToString(), errorTitle);
            }
        }

        public Task SendVerificationCodeViaEmailAsync(string email, string code)
        {
            return Task.FromResult(true);
        }

        public Task SendVerificationCodeViaSmsAsync(string phoneNo, string code)
        {
            return Task.FromResult(true);
        }

        public Task<bool> VerifyCodeAsync(string identifier, string userInputCode)
        {
            if (_cache.TryGetValue(identifier, out string? storedCode) && storedCode == userInputCode)
            {
                // Invalidate code after successful use
                _cache.Remove(identifier);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        Task IVerificationCodeService.SendVerificationCodeViaEmailAsync(string email, string code)
        {
            return SendVerificationCodeViaEmailAsync(email, code);
        }

    }
}