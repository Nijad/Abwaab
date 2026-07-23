using Abwaab.Application.Common.Interfaces;
using Abwaab.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace Abwaab.Infrastructure.Services.SmsServices
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly SmsSettings _settings;
        private readonly ILogger<TwilioSmsSender> _logger;

        public TwilioSmsSender(IOptions<SmsSettings> settings, ILogger<TwilioSmsSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_settings.AccountSid) || string.IsNullOrEmpty(_settings.AuthToken))
                    throw new InvalidOperationException("Twilio credentials are missing.");

                TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(_settings.FromPhone),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                if (!string.IsNullOrEmpty(result.Sid))
                {
                    _logger.LogInformation("SMS sent successfully to {PhoneNumber}. SID: {Sid}", phoneNumber, result.Sid);
                    return true;
                }

                _logger.LogWarning("SMS sent but no SID returned for {PhoneNumber}.", phoneNumber);
                return false;
            }
            catch (ApiException ex)
            {
                // Twilio specific errors (e.g., invalid number, insufficient balance)
                _logger.LogError(ex, "Twilio API error sending SMS to {PhoneNumber}: {Message}", phoneNumber, ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending SMS to {PhoneNumber}.", phoneNumber);
                return false;
            }
        }
    }
}