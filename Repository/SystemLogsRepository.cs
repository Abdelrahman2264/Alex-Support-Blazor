using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;

namespace AlexSupport.Repository
{
    public class SystemLogsRepository : ISystemLogsRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<SystemLogsRepository> logger;
        public SystemLogsRepository(AlexSupportDB alexsupportdb, ILogger<SystemLogsRepository> logger)
        {
            this.alexSupportDB = alexsupportdb;
            this.logger = logger;
        }
        public async Task CreateLog(SystemLogs logsHistory)
        {
            try
            {

                await alexSupportDB.SystemLogs.AddAsync(logsHistory);
                await alexSupportDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error In Create New System Log: {ex.InnerException}", ex);

            }
        }
    }
}

