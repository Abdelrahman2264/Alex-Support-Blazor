using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class LocationRepository:ILocationRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<LocationRepository> logger;
        public LocationRepository(AlexSupportDB alexSupportDB, ILogger<LocationRepository> logger)
        {
            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
        }

        public async Task<Location> CreateLocationAsync(Location Location)
        {
            try
            {
                if (Location != null)
                {
                    Location.isActive = true;
                    Location.CreatedDate = DateTime.Now;
                    await alexSupportDB.Locations.AddAsync(Location);
                    await alexSupportDB.SaveChangesAsync();
                    return Location;
                }
                else
                {
                    logger.LogError("Location is null");
                    return new Location();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Createing Location Async: " + ex.Message, ex);
                return new Location();
            }
        }

        public async Task<bool> DeleteLocationAsync(int Id)
        {
            try
            {
                var Location = await alexSupportDB.Locations.FirstOrDefaultAsync(c => c.LID == Id);
                if (Location != null)
                {
                    Location.isActive = false;
                    await alexSupportDB.SaveChangesAsync();
                    return true;
                }
                logger.LogError("Location Not Found");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Deleting Location Async: " + ex.Message, ex);
                return false;
            }
        }


        public async Task<IEnumerable<Location>> AllLocationsAsync()
        {
            try
            {
                return await alexSupportDB.Locations.OrderBy(c => c.LocationName).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get ALl Locations Async: " + ex.Message, ex);
                return Enumerable.Empty<Location>();
            }
        }

        public async Task<Location> GetLocationAsync(int Id)
        {
            try
            {
                var Location = await alexSupportDB.Locations.FirstOrDefaultAsync(c => c.LID == Id);
                if (Location != null)
                {
                    return Location;
                }
                else
                {
                    logger.LogError("Location not found");
                    return new Location();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get Location Async: " + ex.Message, ex);
                return new Location();
            }



        }

        public async Task<Location> UpdateLocationAsync(Location Location)
        {
            try
            {
                var UpdatedLocation = await alexSupportDB.Locations.FirstOrDefaultAsync(c => c.LID == Location.LID);
                if (UpdatedLocation != null)
                {
                    UpdatedLocation.LocationName = Location.LocationName;
                    UpdatedLocation.isActive = true;
                    alexSupportDB.Locations.Update(UpdatedLocation);
                    await alexSupportDB.SaveChangesAsync();
                    return UpdatedLocation;
                }
                else
                {
                    logger.LogError("Location not found");
                    return new Location();

                }
            }
            catch (Exception ex)
            {

                logger.LogError("Error in Update Location Async: " + ex.Message, ex);
                return Location;
            }
        }
        public async Task<bool> CheckIfSiteNameExist(string name, int id = 0)
        {
            try
            {


                if (id != 0)
                {
                    var site = await alexSupportDB.Locations.FirstOrDefaultAsync(u =>  u.isActive == true && u.LID != id && u.LocationName.ToLower() == name.ToLower());
                    if (site != null)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    var site = await alexSupportDB.Locations.FirstOrDefaultAsync(u => u.LocationName.ToLower() == name.ToLower() && u.isActive == true);
                    if (site != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in Check If Site Code Exist: {ex.Message}", ex);

                return false;
            }
        }
    }
}
