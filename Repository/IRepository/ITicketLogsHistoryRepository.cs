using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ITicketLogsHistoryRepository
    {
        public Task CreateLog(Tlog logsHistory);
        public Task<IEnumerable<Tlog>> GetAllLogsAsync();
    }
}
