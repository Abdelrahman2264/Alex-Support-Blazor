using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class AppUserRepoistory : IAppUserRepoistory
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<AppUserRepoistory> logger;
        public AppUserRepoistory(AlexSupportDB alexSupportDB, ILogger<AppUserRepoistory> logger)
        {
            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
        }
        public async Task<IEnumerable<AppUser>> GetAllAgentsAsync()
        {
            try
            {
                return await alexSupportDB.AppUser
                    .Where(u => u.Role == "Agent")
                    .Include(u => u.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all agents: " + ex.Message, ex);
                return new List<AppUser>();
            }
        }
    }
}
