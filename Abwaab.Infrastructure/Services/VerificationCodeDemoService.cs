using Abwaab.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Abwaab.Infrastructure.Services
{
    public class VerificationCodeDemoService : IVerificationCodeService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly IMemoryCache _cache;
        private readonly ILogger<VerificationCodeService> _logger;

        public VerificationCodeDemoService(
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
            return "123456";
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