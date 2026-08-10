
using Abwaab.Domain.Entities.UserEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyService
    {
        Task<Guid> CreatePropertyAsync(UserPlan userPlan);
        Task<bool> HasBalanceToAddPropertyAsync(UserPlan userPlan);
    }
}
