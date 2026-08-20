using Abwaab.Application.Features.Properties.Queries.GetPropertyForUpdate;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyTimeSlotService
    {
        Task<List<TimeSlotForUpdate>> GetPropertyTimeSlotsListAsync(Guid propertyId);
    }
}
