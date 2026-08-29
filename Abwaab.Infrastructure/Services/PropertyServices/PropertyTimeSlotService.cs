using Abwaab.Application.Common.Exceptions.Properties.TimeSlots;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Features.Properties.Common.DTOs;
using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Services.Common;

namespace Abwaab.Infrastructure.Services.PropertyServices
{
    public class PropertyTimeSlotService : IPropertyTimeSlotService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;

        public PropertyTimeSlotService(ITimeSlotRepository timeSlotRepository)
        {
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<List<TimeSlotDTO>> GetPropertyTimeSlotsListAsync(Guid propertyId)
        {
            List<TimeSlot> timeSlots = await _timeSlotRepository.GetTimeSlotsByPropertyIdAsync(propertyId);

            List<TimeSlotDTO> ptsl = new();
            foreach (var timeSlot in timeSlots)
                ptsl.Add(new()
                {
                    TimeSlotId = timeSlot.Id,
                    DayNumber = (int)timeSlot.Day,
                    StartTime = timeSlot.StartTime,
                    EndTime = timeSlot.EndTime,
                    Notes = timeSlot.Notes
                });

            return ptsl;
        }

        // C#
        public async Task SyncronizePropertyTimeSlotsAsync(List<TimeSlot>? existingTimeSlots, List<TimeSlotDTO>? commingTimeSlots, Guid propertyId)
        {
            await SyncronizingCollection.Sync(
                existingTimeSlots, commingTimeSlots,
                (existing, comming) => existing.Id == comming.TimeSlotId,
                async (existing) => await DeleteTimeSlotAsync(existing),
                async (existing, comming) => await UpdateTimeSlotAsync(existing, comming),
                async (comming) => await AddTimeSlotAsync(comming, propertyId));
        }

        private async Task DeleteTimeSlotAsync(TimeSlot existing)
        {
            await _timeSlotRepository.DeleteTimeSlotAsync(existing);
        }

        private async Task UpdateTimeSlotAsync(TimeSlot existing, TimeSlotDTO comming)
        {
            existing.Day = comming.DayNumber;
            existing.StartTime = comming.StartTime;
            existing.EndTime = comming.EndTime;
            existing.Notes = comming.Notes;

            await _timeSlotRepository.UpdateTimeSlotAsync(existing);
        }

        private async Task AddTimeSlotAsync(TimeSlotDTO comming, Guid propetyId)
        {
            TimeSlot timeSlot = new()
            {
                Id = Guid.NewGuid(),
                PropertyId = propetyId,
                Day = comming.DayNumber,
                StartTime = comming.StartTime,
                EndTime = comming.EndTime,
                Notes = comming.Notes
            };

            await _timeSlotRepository.AddTimeSlotAsync(timeSlot);
        }

        public async Task<TimeSlot> FindTimeSlotByIdAsync(Guid? timeSlotId, string errorTitle)
        {
            TimeSlot? timeSlot = await _timeSlotRepository.FindTimeSlotByIdAsync(timeSlotId);
            if (timeSlot == null)
                throw new TimeSlotNotFoundException(errorTitle);
            return timeSlot;
        }
    }
}
