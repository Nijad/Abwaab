using Abwaab.Application.Common.Exceptions.Email;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Abwaab.Infrastructure.Services.EmailServices
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(IOptions<EmailSettings> settings, ILogger<SendGridEmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string errorTitle)
        {
            try
            {
                if (string.IsNullOrEmpty(_settings.SendGridApiKey))
                    throw new InvalidOperationException("SendGrid API Key is missing.");

                var client = new SendGridClient(_settings.SendGridApiKey);
                var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent successfully to {ToEmail}.", toEmail);
                    //return true;
                }

                var errorBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("SendGrid failed: {StatusCode} - {Error}", response.StatusCode, errorBody);
                //return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending email to {ToEmail}.", toEmail);
                //return false;
                throw new FailedSendignEmailException(errorTitle);
            }
        }
    }
}
