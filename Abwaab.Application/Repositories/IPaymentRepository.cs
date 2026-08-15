using Abwaab.Application.Common.Enums;
using Abwaab.Domain.Entities.PaymentEntities;

namespace Abwaab.Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<ServiceType?> FindServiceTypeByName(ServiceTypesEnum plan_Subscription);
    }
}
