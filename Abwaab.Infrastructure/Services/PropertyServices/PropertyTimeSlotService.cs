using Abwaab.Application.Contracts;
using Abwaab.Application.Features.Properties.Common;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyTimeSlotService : IPropertyTimeSlotService
    {
        private readonly IPropertyRepository _propertyRepository;

        public PropertyTimeSlotService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<List<TimeSlotDTO>> GetPropertyTimeSlotsListAsync(Guid propertyId)
        {
            List<TimeSlot> timeSlots = await _propertyRepository.GetTimeSlotsByPropertyIdAsync(propertyId);

            List<TimeSlotDTO> ptsl = new();
            foreach (var timeSlot in timeSlots)
                ptsl.Add(new()
                {
                    TimeSlotId = timeSlot.Id,
                    Day = (int)timeSlot.Day,
                    StartTime = timeSlot.StartTime,
                    EndTime = timeSlot.EndTime,
                    Notes = timeSlot.Notes
                });

            return ptsl;
        }
    }
}
