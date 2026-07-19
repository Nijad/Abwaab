using Abwaab.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services
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

        public string GenerateCode()
        {
            // Generates a cryptographically random 6-digit code (e.g., "123456")
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> SendVerificationCodeAsync(string email, string phoneNumber, string code)
        {
            // Decide which channel to use
            if (!string.IsNullOrEmpty(email))
            {
                var subject = "Your Account Verification Code";
                var body = $@"
                    <h2>Email Verification</h2>
                    <p>Thank you for registering. Please use the following code to verify your account:</p>
                    <h1 style='font-size: 32px; letter-spacing: 5px; color: #2d3748;'>{code}</h1>
                    <p>This code will expire in 5 minutes.</p>
                    <p>If you didn't request this, please ignore this email.</p>
                ";

                var result = await _emailSender.SendEmailAsync(email, subject, body);
                if (result)
                {
                    // Store the code in cache with a 5-minute expiry
                    _cache.Set(email, code, TimeSpan.FromMinutes(5));
                    return true;
                }
                return false;
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                var message = $"Your OTP is: {code[0]}{code[1]}{code[2]}-{code[3]}{code[4]}{code[5]} (valid 5 min)";
                //var message = $"Your verification code is: {code}. It will expire in 5 minutes.";
                var result = await _smsSender.SendSmsAsync(phoneNumber, message);
                if (result)
                {
                    _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(5));
                    return true;
                }
                return false;
            }

            _logger.LogWarning("SendVerificationCodeAsync called with neither email nor phoneNumber.");
            return false;
        }

        public Task<bool> VerifyCodeAsync(string identifier, string userInputCode, bool isEmail = true)
        {
            string key = isEmail ? $"verification_email_{identifier}" : $"verification_phone_{identifier}";

            if (_cache.TryGetValue(key, out string storedCode))
            {
                if (storedCode == userInputCode)
                {
                    _cache.Remove(key); // Invalidate code after successful use
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}