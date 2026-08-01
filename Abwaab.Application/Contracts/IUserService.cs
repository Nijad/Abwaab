using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Contracts
{
    public interface IUserService
    {
        Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifierEnum identifierType);
    }
}
