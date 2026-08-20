using Abwaab.Application.Features.Properties.Common;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyTimeSlotService
    {
        Task<List<TimeSlotDTO>> GetPropertyTimeSlotsListAsync(Guid propertyId);
    }
}
