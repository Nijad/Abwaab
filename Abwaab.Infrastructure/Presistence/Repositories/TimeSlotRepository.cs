using Abwaab.Application.Repositories;
using Abwaab.Domain.Entities.PropertyEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly AppDbContext _context;

        public TimeSlotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTimeSlotAsync(TimeSlot timeSlot)
        {
            await _context.TimeSlots.AddAsync(timeSlot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimeSlotAsync(TimeSlot existing)
        {
            _context.TimeSlots.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimeSlotAsync(TimeSlot existing)
        {
            _context.TimeSlots.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TimeSlot>> GetTimeSlotsByPropertyIdAsync(Guid propertyId)
        {
            return await _context.TimeSlots.Where(x => x.PropertyId == propertyId).ToListAsync();
        }

        public async Task<TimeSlot?> FindTimeSlotByIdAsync(Guid? timeSlotId)
        {
            return await _context.TimeSlots.Where(x=>x.Id==timeSlotId).FirstOrDefaultAsync();
        }
    }
}
