using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Common.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task CreateAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
        Task RevokeAsync(string token, string? revokedByIp = null);
        Task<IEnumerable<RefreshToken>> GetActiveTokensForUserAsync(Guid userId);
    }
}
