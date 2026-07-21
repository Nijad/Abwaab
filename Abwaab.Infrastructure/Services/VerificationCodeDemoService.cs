using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Infrastructure.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services
{
    public class VerificationCodeDemoService : IVerificationCodeService
    {
        private readonly IMemoryCache _cache;

        public VerificationCodeDemoService(
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache,
            ILogger<VerificationCodeService> logger)
        {
            _cache = cache;
        }

        public string GenerateCode()
        {
            return "123456";
        }

        public Task<ResendCodeResponse> ResendVerificationCodeAsync(string identifier)
        {
            string code = GenerateCode();
            if (CommonValidation.IsValidEmail(identifier))
            {
                return SendVerificationCodeAsync(identifier, null, code)
                    .ContinueWith(task => new ResendCodeResponse
                    {
                        IsSuccess = task.Result,
                        Message = task.Result ? "Verification code resent to email." : "Failed to resend verification code to email."
                    });
            }
            else if (CommonValidation.IsValidPhoneNumber(identifier))
            {
                return SendVerificationCodeAsync(null, identifier, code)
                    .ContinueWith(task => new ResendCodeResponse
                    {
                        IsSuccess = task.Result,
                        Message = task.Result ? "Verification code resent to phone number." : "Failed to resend verification code to phone number."
                    });
            }
            else
            {
                return Task.FromResult(new ResendCodeResponse
                {
                    IsSuccess = false,
                    Message = "Invalid identifier. Must be a valid email or phone number."
                });
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string email, string phoneNumber, string code)
        {
            if (!string.IsNullOrEmpty(email))
                _cache.Set(email, code, TimeSpan.FromMinutes(5));
            else if (!string.IsNullOrEmpty(phoneNumber))
                _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(5));
            return true;
        }

        public Task<bool> VerifyCodeAsync(string identifier, string userInputCode)
        {
            if (_cache.TryGetValue(identifier, out string? storedCode))
            {
                if (storedCode == userInputCode)
                {
                    _cache.Remove(identifier); // Invalidate code after successful use
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}