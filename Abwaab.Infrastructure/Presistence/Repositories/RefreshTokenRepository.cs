using Abwaab.Application.Common.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == HashToken(token));
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        //public async Task RevokeAsync(string token, string? revokedByIp = null)
        //{
        //    var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        //    if (refreshToken != null)
        //    {
        //        refreshToken.IsRevoked = true;
        //        refreshToken.RevokedByIp = revokedByIp;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<IEnumerable<RefreshToken>> GetActiveTokensForUserAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task<RefreshToken?> GetByHashAsync(string hash)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hash);
        }

        public async Task RevokeAsync(string rawToken, string? revokedByIp = null)
        {
            var hash = HashToken(rawToken);
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hash);
            if (token != null)
            {
                token.IsRevoked = true;
                token.RevokedByIp = revokedByIp;
                await _context.SaveChangesAsync();
            }
        }

        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
