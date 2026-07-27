using Abwaab.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Abwaab.Infrastructure.Services.Common
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IMemoryCache _cache;

        public TokenBlacklistService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddToBlacklist(string jti, DateTime expiry)
        {
            var ttl = expiry - DateTime.UtcNow;
            if (ttl > TimeSpan.Zero)
            {
                _cache.Set($"blacklist_{jti}", true, ttl);
            }
        }

        public bool IsBlacklisted(string jti)
        {
            return _cache.TryGetValue($"blacklist_{jti}", out _);
        }
    }
}