using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlexSupport.Repository
{
    public class DailyTasksRepository : IDailyTaskRepository
    {
        private readonly AlexSupportDB alexsupportdb;
        private readonly ILogger<DailyTasksRepository> _logger;
        public DailyTasksRepository(AlexSupportDB alexsupportdb, ILogger<DailyTasksRepository> _logger)
        {
            this.alexsupportdb = alexsupportdb;
            this._logger = _logger;

        }
        public async Task<DailyTasks> CreateDailyTaskAsync(DailyTasks dailytask)
        {
            try
            {
                if (dailytask != null)
                {
                    await alexsupportdb.DailyTasks.AddAsync(dailytask);
                    await alexsupportdb.SaveChangesAsync();
                    return dailytask;
                }
                return new DailyTasks();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create a Dailt Task: {ex.Message}", ex);
                return new DailyTasks();
            }
        }

        public async Task<IEnumerable<DailyTasks>> GetAllDailyTasksAsync()
        {
            try
            {
                return await alexsupportdb.DailyTasks
                    .Where(u => u.IsActive == true)
                    .Include(c => c.category)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting all Daily Tasks: " + ex.Message, ex);
                return Enumerable.Empty<DailyTasks>();
            }

        }

        public async Task<DailyTasks> GetDailyTaskAsync(int id)
        {
            try
            {
                var DailyTask = await alexsupportdb.DailyTasks.FirstOrDefaultAsync(u => u.DTID == id);
                if (DailyTask != null)
                {
                    return DailyTask;
                }
                return new DailyTasks();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting Daily Task: " + ex.Message, ex);
                return new DailyTasks();
            }
        }



        public async Task<bool> InActiveDailyTaskAsync(int id)
        {
            try
            {
                var DailyTask = await alexsupportdb.DailyTasks.FirstOrDefaultAsync(u => u.DTID == id);
                if (DailyTask != null)
                {
                    DailyTask.IsActive = false;
                    alexsupportdb.DailyTasks.Update(DailyTask);
                    await alexsupportdb.SaveChangesAsync();
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in InActive Daily Task: " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> UpdateDailyTaskAsync(DailyTasks dailytask)
        {
            try
            {
                var UpdatedDailyTask = await alexsupportdb.DailyTasks.FirstOrDefaultAsync(u => u.DTID == dailytask.DTID);
                if (UpdatedDailyTask != null)
                {
                    UpdatedDailyTask.Subject = dailytask.Subject;
                    UpdatedDailyTask.Priority = dailytask.Priority;
                    UpdatedDailyTask.Issue = dailytask.Issue;
                    UpdatedDailyTask.CategoryID = dailytask.CategoryID;
                    UpdatedDailyTask.Due_Minutes = dailytask.Due_Minutes;
                    alexsupportdb.DailyTasks.Update(UpdatedDailyTask);
                    await alexsupportdb.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Update Daily Task: " + ex.Message, ex);
                return false;
            }
        }
    }
}
