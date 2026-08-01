namespace Abwaab.Application.Interfaces
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
        public string? RemoteIpAddress { get; }
        bool IsInRole(string role);
    }
}
