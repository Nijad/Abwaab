using Abwaab.Application.Repositories;
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
        public async Task<NotificationWay?> GetNotificationWayByNameAsync(string wayName)
        {
            return await _context.NotificationWays.Where(nw => nw.WayName == wayName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NotificationWay>> GetNotificationAllWaysAsync()
        {
            return await _context.NotificationWays.ToListAsync();
        }

        public async Task<List<UserNotificationSubscription>> GetNotificationWaysByUserAsync(Guid userId)
        {
            return await _context.UserNotificationSubscriptions.Where(nw => nw.UserId == userId).ToListAsync();
        }

        public async Task<NotificationWay?> GetNotificationWayByIdAsync(Guid id)
        {
            return await _context.NotificationWays.FindAsync(id);
        }

        public async Task<UserNotificationSubscription?> GetUserSubscriptionAsync(Guid userId, Guid notifiactionWayId)
        {
            return await _context.UserNotificationSubscriptions.Where(x => x.UserId == userId && x.NotificationWayId == notifiactionWayId).FirstOrDefaultAsync();
        }

        public async Task UpdateSubscriptionAsync(UserNotificationSubscription userSubscription)
        {
            _context.UserNotificationSubscriptions.Update(userSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubscriptionAsync(UserNotificationSubscription userSubscription)
        {
            await _context.AddAsync(userSubscription);
            await _context.SaveChangesAsync();
        }
    }
}
