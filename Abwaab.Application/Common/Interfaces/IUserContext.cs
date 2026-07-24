namespace Abwaab.Application.Common.Interfaces
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        public string? RemoteIpAddress { get; }
    }
}
