using AlexSupport.ViewModels;
using Microsoft.Extensions.Logging;

namespace AlexSupport.Repository.IRepository
{
    public interface ILocationRepository
    {
        public Task<Location> CreateLocationAsync(Location Location);
        public Task<Location> UpdateLocationAsync(Location Location);
        public Task<Location> GetLocationAsync(int Id);
        public Task<IEnumerable<Location>> AllLocationsAsync();
        public Task<IEnumerable<Location>> AllSystemLocationsAsync();
        public Task<bool> ActivateLocationAsync(int Id);
        public Task<bool> InActivateLocationAsync(int Id);
        public Task<bool> CheckIfSiteNameExist(string code, int id = 0);

    }
}
