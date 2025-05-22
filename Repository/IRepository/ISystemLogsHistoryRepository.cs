using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ISystemLogsRepository
    {
        public Task CreateLog(SystemLogs logsHistory);
    }
}
