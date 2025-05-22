using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class TicketLogsHistoryRepository : ITicketLogsHistoryRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<TicketLogsHistoryRepository> logger;
        public TicketLogsHistoryRepository(AlexSupportDB alexsupportdb, ILogger<TicketLogsHistoryRepository> logger)
        {
            this.alexSupportDB = alexsupportdb;
            this.logger = logger;
        }
        public async Task CreateLog(Tlog logsHistory)
        {
            try
            {

                await alexSupportDB.Tlogs.AddAsync(logsHistory);
                await alexSupportDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error In Create New Log: {ex.Message}", ex);

            }
        }



        public async Task<IEnumerable<Tlog>> GetAllLogsAsync()
        {
            try
            {
                return await alexSupportDB.Tlogs
                    .Include(u => u.AppUser)
                    .Include(u => u.Ticket)
                    .ToListAsync();
            }

            catch (Exception ex)
            {
                logger.LogError($"Error In Get All Logs: {ex.Message}", ex);
                return Enumerable.Empty<Tlog>();
            }
        }
    }
}
