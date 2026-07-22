using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
