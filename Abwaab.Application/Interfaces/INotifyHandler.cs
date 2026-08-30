namespace Abwaab.Application.Interfaces
{
    public interface INotifyHandler
    {
        Task NotifyAsync(string errorTitle);
    }
}
