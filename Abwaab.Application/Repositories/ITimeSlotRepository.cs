using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Application.Repositories
{
    public interface ITimeSlotRepository
    {
        Task AddTimeSlotAsync(TimeSlot timeSlot);
        Task DeleteTimeSlotAsync(TimeSlot existing);
        Task UpdateTimeSlotAsync(TimeSlot existing);
        Task<List<TimeSlot>> GetTimeSlotsByPropertyIdAsync(Guid propertyId);
        Task<TimeSlot?> FindTimeSlotByIdAsync(Guid? timeSlotId);
    }
}
