
using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface IDailyTaskRepository
    {
        public Task<DailyTasks> CreateDailyTaskAsync(DailyTasks dailytask);
        public Task<bool> UpdateDailyTaskAsync(DailyTasks dailytask);
        public Task<DailyTasks> GetDailyTaskAsync(int id);
        public Task<bool> InActiveDailyTaskAsync(int id);
        public Task<IEnumerable<DailyTasks>> GetAllDailyTasksAsync();
    }
}
