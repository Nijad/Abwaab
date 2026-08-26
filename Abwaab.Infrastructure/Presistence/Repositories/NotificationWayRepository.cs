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

        public async Task<IEnumerable<NotificationWay>> GetAllNotificationWaysAsync(bool onlyCanDisable = true)
        {
            IQueryable<NotificationWay> query = _context.NotificationWays.Where(x => x.WayName != "");
            if (onlyCanDisable)
                query = query.Where(x => x.CanDisable);
            return await query.ToListAsync();
        }

        public async Task<List<UserNotificationSubscription>> GetNotificationWaysByUserAsync(Guid userId, bool activeOnly = false)
        {
            IQueryable<UserNotificationSubscription> list = _context.UserNotificationSubscriptions
                .Where(nw => nw.UserId == userId);

            if (activeOnly)
                list = list.Where(x => x.IsInactive == false);
            
            return await list.ToListAsync();
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

        public async Task<List<UserNotificationSubscription>> GetAllNotificationWaysOfUserAsync(Guid userId)
        {
            return await _context.UserNotificationSubscriptions.Include(x => x.NotificationWay).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<bool> HasUserActiveNotificationWay(Guid userId, Guid notifiacationWayId)
        {
            UserNotificationSubscription? subscription = await _context.UserNotificationSubscriptions.Where(x => x.UserId == userId && x.NotificationWayId == notifiacationWayId).FirstOrDefaultAsync();

            return !(subscription == null || subscription.IsInactive);
        }

        public async Task<NotificationState?> FindNotificationStateByStateNameAsync(string notificationStateName)
        {
            return await _context.NotificationStates.Where(x=>x.StateName== notificationStateName).FirstOrDefaultAsync();
        }

        public async Task AddNotificationsRangeAsync(List<Notification> notifications)
        {
            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
