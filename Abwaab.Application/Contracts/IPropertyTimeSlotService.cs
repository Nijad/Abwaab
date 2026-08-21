using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts
{
    public interface IPropertyTimeSlotService
    {
        Task<List<TimeSlotDTO>> GetPropertyTimeSlotsListAsync(Guid propertyId);
        Task SyncronizePropertyTimeSlotsAsync(List<TimeSlot>? existingTimeSlots, List<TimeSlotDTO>? commingTimeSlots, Guid propertyId);
    }
}
