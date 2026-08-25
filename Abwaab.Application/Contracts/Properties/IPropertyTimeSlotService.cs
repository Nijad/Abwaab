using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Contracts.Properties
{
    public interface IPropertyTimeSlotService
    {
        Task<TimeSlot> FindTimeSlotByIdAsync(Guid? timeSlotId, string errorTitle);
        Task<List<TimeSlotDTO>> GetPropertyTimeSlotsListAsync(Guid propertyId);
        Task SyncronizePropertyTimeSlotsAsync(List<TimeSlot>? existingTimeSlots, List<TimeSlotDTO>? commingTimeSlots, Guid propertyId);
    }
}
