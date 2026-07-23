using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Domain.Enums;
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

        public Task<ResendCodeResponse> ResendVerificationCodeAsync(IdentifierDTO resendCodeDTO)
        {
            string code = GenerateCode();
            if (resendCodeDTO.IdentifierType == IdentifierEnum.email)
            {
                return SendVerificationCodeAsync(resendCodeDTO, code)
                    .ContinueWith(task => new ResendCodeResponse
                    {
                        IsSuccess = task.Result,
                        Message = task.Result ? "Verification code resent to email." : "Failed to resend verification code to email."
                    });
            }
            else if (resendCodeDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                return SendVerificationCodeAsync(resendCodeDTO, code)
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

        public async Task<bool> SendVerificationCodeAsync(IdentifierDTO Identifier, string code)
        {
            _cache.Set(Identifier.Identifier, code, TimeSpan.FromMinutes(5));
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