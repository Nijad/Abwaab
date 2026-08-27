namespace Abwaab.Application.Interfaces
{
    public interface ISmsSender
    {
        Task<(bool, string)> SendSmsAsync(string phoneNumber, string message, string errorTitle);
    }
}
