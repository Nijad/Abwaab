namespace Abwaab.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}
