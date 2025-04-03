using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface IAppUserRepoistory
    {
        public Task<IEnumerable<AppUser>> GetAllAgentsAsync();
    }
}
