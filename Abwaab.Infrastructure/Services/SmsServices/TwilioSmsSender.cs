using Abwaab.Application.Common.Exceptions.SMS;
using Abwaab.Application.Interfaces;
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

        public async Task SendSmsAsync(string phoneNumber, string message,string errorTitle)
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
                    _logger.LogInformation($"SMS sent successfully to {phoneNumber}. SID: {result.Sid}");
                }

                _logger.LogWarning($"SMS sent but no SID returned for {phoneNumber}.");
            }
            catch (ApiException ex)
            {
                // Twilio specific errors (e.g., invalid number, insufficient balance)
                _logger.LogError(ex, $"Twilio API error sending SMS to {phoneNumber}: {ex.Message}");
                throw new FailedSendignSMSException(errorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while sending SMS to {phoneNumber}.");
                throw new FailedSendignSMSException(errorTitle);
            }
        }
    }
}