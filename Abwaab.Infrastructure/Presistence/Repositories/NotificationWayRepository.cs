using Abwaab.Application.Common.Interfaces;
using Abwaab.Domain.Entities.NotificationEntities;
using Abwaab.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Abwaab.Infrastructure.Presistence.Repositories
{
    public class NotificationWayRepository : INotificationWayRepository
    {
        private readonly AppDbContext _context;
        public NotificationWayRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<NotificationWay?> GetNotificationWay(string wayName)
        {
            return await _context.NotificationWays.Where(nw => nw.WayName == wayName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NotificationWay>> GetNotificationWays()
        {
            return await _context.NotificationWays.ToListAsync();
        }

        public async Task<List<UserNotificationSubscription>> GetUserNotificationWays(Guid userId)
        {
            return await _context.UserNotificationSubscriptions.Where(nw => nw.UserId == userId).ToListAsync();
        }
    }
}
