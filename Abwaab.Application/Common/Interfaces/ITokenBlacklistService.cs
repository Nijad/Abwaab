namespace Abwaab.Application.Common.Interfaces
{
    public interface ITokenBlacklistService
    {
        void AddToBlacklist(string jti, DateTime expiry);
        bool IsBlacklisted(string jti);
    }
}
