using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class NotificationRepository : INotificationRepoisitory
    {
        private readonly AlexSupportDB alexsupportdb;
        private readonly ILogger<NotificationRepository> logger;
        public NotificationRepository(AlexSupportDB alexsupportdb, ILogger<NotificationRepository> logger)
        {
            this.alexsupportdb = alexsupportdb;
            this.logger = logger;
        }
        public async Task<SystemNotification> CreateNotificationAsync(SystemNotification note)
        {
            try
            {
                if (note == null)
                {
                    logger.LogError("Notification is null");
                    return new SystemNotification();
                }
                note.SentAt = DateTime.Now;
                note.IsRead = false;
                await alexsupportdb.Notifications.AddAsync(note);
                await alexsupportdb.SaveChangesAsync();
                logger.LogInformation($"Notification created with ID: {note.NID}");
                return note;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating notification" + ex.Message);
                return new SystemNotification();
            }

        }
        public async Task<bool> ExistsRecentNotificationAsync(string message, int fromUserId, int toUserId, TimeSpan timeWindow)
        {
            var timeLimit = DateTime.Now.Subtract(timeWindow);
            return await alexsupportdb.Notifications.AnyAsync(n =>
                n.Message == message &&
                n.FromUserId == fromUserId &&
                n.ToUserId == toUserId &&
                n.SentAt >= timeLimit);
        }


        public async Task<List<SystemNotification>> GetAllUnReadNotificationAsyncForUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    logger.LogError("Invalid user ID");
                    return new List<SystemNotification>();
                }
                var notifications = await alexsupportdb.Notifications
                    .Where(n => n.ToUserId == id && !n.IsRead)
                    .OrderByDescending(n => n.SentAt)
                    .ToListAsync();
                logger.LogInformation($"Retrieved {notifications.Count} unread notifications for user ID: {id}");
                return notifications;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error reading notification" + ex.Message);
                return new List<SystemNotification>();
            }
        }

        public async Task<SystemNotification> GetNotificationAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    logger.LogError("Invalid Notification ID");
                    return new SystemNotification();
                }
                var notification = await alexsupportdb.Notifications
                    .FirstOrDefaultAsync(n => n.NID == id && !n.IsRead);
                if (notification == null)
                {
                    logger.LogWarning($"Notification with ID {id} not found.");
                    return new SystemNotification();
                }
                return notification;
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving notification" + ex.Message);
                return new SystemNotification();
            }

        }

        public async Task<SystemNotification> ReadNotificationAsync(SystemNotification note)
        {

            try
            {
                if (note == null)
                {
                    logger.LogError("Notification is null");
                    return new SystemNotification();
                }
                var existingNote = await alexsupportdb.Notifications.FirstOrDefaultAsync(n => n.NID == note.NID);
                if (existingNote == null)
                {
                    logger.LogWarning($"Notification with ID {note.NID} not found.");
                    return new SystemNotification();
                }
                existingNote.IsRead = true;
                existingNote.ReadAt = DateTime.UtcNow;
                alexsupportdb.Notifications.Update(existingNote);
                await alexsupportdb.SaveChangesAsync();
                logger.LogInformation($"Notification with ID {note.NID} marked as read.");
                return existingNote;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error reading notification" + ex.Message);
                return new SystemNotification();

            }
        }
    }
}
