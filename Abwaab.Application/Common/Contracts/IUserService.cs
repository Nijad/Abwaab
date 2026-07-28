using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Domain.Enums;

namespace Abwaab.Application.Common.Contracts
{
    public interface IUserService
    {
        Task<ApplicationUser?> FindUserByIdentifierAsync(string identifier, IdentifierEnum identifierType);
    }
}
