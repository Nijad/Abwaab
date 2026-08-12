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

        public async Task<SendCodeResponse> SendVerificationCodeAsync(SendCodeDTO resendCodeDTO)
        {
            if (resendCodeDTO.IdentifierType == IdentifiersEnum.Email)
            {
                // Send the code to the email
                return await SendVerificationCodeViaEmailAsync(resendCodeDTO.Identifier, resendCodeDTO.Code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "Verification code resent to email."
                    });
            }
            else if (resendCodeDTO.IdentifierType == IdentifiersEnum.Phone_Number)
            {
                // Send the code to the phone number
                return await SendVerificationCodeViaSmsAsync(resendCodeDTO.Identifier, resendCodeDTO.Code)
                    .ContinueWith(task => new SendCodeResponse
                    {
                        Success = true,
                        Message = "Verification code resent to phone number."
                    });
            }

            throw new NotImplementedIdentifierException(resendCodeDTO.IdentifierType.ToString());
        }

        public async Task SendVerificationCodeViaEmailAsync(string email, string code)
        {
            var subject = "Your Account Verification Code";
            var body = $@"
                <h2>Email Verification</h2>
                <p>Thank you for registering. Please use the following code to verify your account:</p>
                <h1 style='font-size: 32px; letter-spacing: 5px; color: #2d3748;'>{code}</h1>
                <p>This code will expire in {GeneralConstants.CODE_TIMEOUT_MINUTES} minutes.</p>
                <p>If you didn't request this, please ignore this email.</p>
            ";

            await _emailSender.SendEmailAsync(email, subject, body);
        }

        public async Task SendVerificationCodeViaSmsAsync(string phoneNo, string code)
        {
            var message = $"Your OTP is: {code[0]}{code[1]}{code[2]}-{code[3]}{code[4]}{code[5]} (valid {GeneralConstants.CODE_TIMEOUT_MINUTES} min)";

            await _smsSender.SendSmsAsync(phoneNo, message);
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