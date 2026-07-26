using Abwaab.Application.Common.Interfaces;
using Abwaab.Application.DTOs.ApplicationUser;
using Abwaab.Application.DTOs.ApplicationUser.VerificationCode;
using Abwaab.Domain.Enums;
using Abwaab.Infrastructure.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services.UserServices
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;
        private readonly ILogger<VerificationCodeService> _logger;
        
        public VerificationCodeService(
            IEmailSender emailSender,
            ISmsSender smsSender,
            IMemoryCache cache,
            ILogger<VerificationCodeService> logger)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
            _cache = cache;
            _logger = logger;
        }

        public string GenerateVerificationCode()
        {
            // Generates a cryptographically random 6-digit code (e.g., "123456")
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public Task<ResendCodeResponse> ResendVerificationCodeAsync(IdentifierDTO resendCodeDTO)
        {
            string code = GenerateVerificationCode();
            if(resendCodeDTO.IdentifierType == IdentifierEnum.email)
            {
                // Send the code to the email
                return SendVerificationCodeViaEmailAsync(resendCodeDTO.Identifier, code)
                    .ContinueWith(task => new ResendCodeResponse
                    {
                        IsSuccess = task.Result,
                        Message = task.Result ? "Verification code resent to email." : "Failed to resend verification code to email."
                    });
            }
            else if(resendCodeDTO.IdentifierType == IdentifierEnum.phone_number)
            {
                // Send the code to the phone number
                return SendVerificationCodeViaSmsAsync(resendCodeDTO.Identifier, code)
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

        public async Task<bool> SendVerificationCodeViaEmailAsync(string email, string code)
        {
            var subject = "Your Account Verification Code";
            var body = $@"
                <h2>Email Verification</h2>
                <p>Thank you for registering. Please use the following code to verify your account:</p>
                <h1 style='font-size: 32px; letter-spacing: 5px; color: #2d3748;'>{code}</h1>
                <p>This code will expire in {Constants.CODE_TIMEOUT_MINUTES} minutes.</p>
                <p>If you didn't request this, please ignore this email.</p>
            ";
            var result = await _emailSender.SendEmailAsync(email, subject, body);
            if (result)
            {
                // Store the code in cache with a 5-minute expiry
                _cache.Set(email, code, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));
                return true;
            }
            return false;
        }

        public async Task<bool> SendVerificationCodeViaSmsAsync(string phoneNo, string code)
        {
            var message = $"Your OTP is: {code[0]}{code[1]}{code[2]}-{code[3]}{code[4]}{code[5]} (valid {Constants.CODE_TIMEOUT_MINUTES} min)";
            //var message = $"Your verification code is: {code}. It will expire in 5 minutes.";
            var result = await _smsSender.SendSmsAsync(phoneNo, message);
            if (result)
            {
                _cache.Set(phoneNo, code, TimeSpan.FromMinutes(Constants.CODE_TIMEOUT_MINUTES));
                return true;
            }
            return false;
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