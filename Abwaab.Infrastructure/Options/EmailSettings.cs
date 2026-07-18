namespace Abwaab.Infrastructure.Options
{
    public class EmailSettings
    {
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;

        // SendGrid Settings
        public string? SendGridApiKey { get; set; }

        // SMTP Settings (alternative)
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
