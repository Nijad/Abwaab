namespace Abwaab.Application.Interfaces
{
    public interface ITokenCacheService
    {
        void AddToBlacklist(string jti, DateTime expiry);
        bool IsBlacklisted(string jti);
    }
}
