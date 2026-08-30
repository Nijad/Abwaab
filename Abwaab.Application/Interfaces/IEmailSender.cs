namespace Abwaab.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<(bool, string)> SendEmailAsync(string toEmail, string subject, string body, string errorTitle);
    }
}
