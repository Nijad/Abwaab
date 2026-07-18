namespace Abwaab.Application.Common.Interfaces
{
    public interface ISmsSender
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
}
