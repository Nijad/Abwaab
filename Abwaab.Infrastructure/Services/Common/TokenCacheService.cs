using Abwaab.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Infrastructure.Services.Common
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly IMemoryCache _cache;

        public TokenCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddToBlacklist(string jti, DateTime expiry)
        {
            TimeSpan ttl = expiry - DateTime.UtcNow;
            if (ttl > TimeSpan.Zero)
                _cache.Set($"blacklist_{jti}", true, ttl);
        }

        public bool IsBlacklisted(string jti)
        {
            return _cache.TryGetValue($"blacklist_{jti}", out _);
        }
    }
}