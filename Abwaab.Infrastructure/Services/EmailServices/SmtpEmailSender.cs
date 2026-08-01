using Abwaab.Application.Common.Exceptions.Email;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Abwaab.Infrastructure.Services.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<EmailSettings> settings, ILogger<SmtpEmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Validate settings
                if (string.IsNullOrEmpty(_settings.SmtpServer))
                    throw new InvalidOperationException("SMTP Server is not configured.");
                if (string.IsNullOrEmpty(_settings.FromEmail))
                    throw new InvalidOperationException("FromEmail is not configured.");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.FromName ?? "No-Reply", _settings.FromEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();

                // Choose the correct SecureSocketOptions based on the port
                SecureSocketOptions options;
                
                // Direct SSL (old standard)
                if (_settings.SmtpPort == 465)
                    options = SecureSocketOptions.SslOnConnect;
                
                // Upgrade to TLS after plain connection
                else if (_settings.SmtpPort == 587)
                    options = SecureSocketOptions.StartTls;
                
                // Fallback: try StartTls if not explicitly set (common for other ports)
                else
                    options = SecureSocketOptions.StartTlsWhenAvailable;

                // Connect with the correct security option
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, options);

                // Authenticate if credentials are provided
                if (!string.IsNullOrEmpty(_settings.SmtpUsername) && !string.IsNullOrEmpty(_settings.SmtpPassword))
                    await client.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {ToEmail} via SMTP.", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP send failed to {ToEmail}.", toEmail);
                throw new FailedSendignEmailException();
            }
        }
    }
}
