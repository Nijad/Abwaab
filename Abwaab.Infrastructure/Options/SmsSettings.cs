namespace Abwaab.Infrastructure.Options
{
    public class SmsSettings
    {
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string FromPhone { get; set; } = string.Empty;
    }
}
