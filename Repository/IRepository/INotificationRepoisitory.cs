using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface INotificationRepoisitory
    {
        public Task<SystemNotification> CreateNotificationAsync(SystemNotification note);
        public Task<SystemNotification> ReadNotificationAsync(SystemNotification note);
        public Task<SystemNotification> GetNotificationAsync(int id);
        public Task<List<SystemNotification>> GetAllUnReadNotificationAsyncForUser(int id);
        public Task<bool> ExistsRecentNotificationAsync(string message, int fromUserId, int toUserId, TimeSpan timeWindow);

    }
}
