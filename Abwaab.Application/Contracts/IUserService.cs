using Abwaab.Application.Features.Users.Auth.Logout;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Contracts
{
    public interface IUserService
    {
        Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifierEnum identifierType);

        string? FindUserByContext();

        string? GetUserJti();

        string? GetUserExpClaim();

        void RemoveCookie(string cookieName);

        Task<LogoutResponse> RevokeActiveToken(Guid userId, bool revokeAll);
        Task AssignDefaultPlantAsync(Guid userId);
    }
}
