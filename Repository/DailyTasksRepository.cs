using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AlexSupport.Repository
{
    public class DailyTasksRepository : IDailyTaskRepository
    {
        private readonly AlexSupportDB alexsupportdb;
        private readonly ILogger<DailyTasksRepository> _logger;
        private readonly ILogService LogService;
        private readonly INotificationService notificationService;
        public DailyTasksRepository(AlexSupportDB alexsupportdb, ILogger<DailyTasksRepository> _logger, ILogService logService, INotificationService notificationService)
        {
            this.alexsupportdb = alexsupportdb;
            this._logger = _logger;
            this.LogService = logService;
            this.notificationService = notificationService;

        }
        public async Task<DailyTasks> CreateDailyTaskAsync(DailyTasks dailytask)
        {
            try
            {
                if (dailytask != null)
                {

                    dailytask.IsActive = true;
                    dailytask.CreatedDate = DateTime.Now;
                    dailytask.LastUpdatedDate = DateTime.Now;
                    await alexsupportdb.DailyTasks.AddAsync(dailytask);
                    await alexsupportdb.SaveChangesAsync();
                    await LogService.CreateSystemLogAsync($"Create A New Daily Task With Id: {dailytask.DTID} In The System", "DAILY TASK");
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
                    .Include(c => c.Category)
                    .Include(c => c.Agent)
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
                    await LogService.CreateSystemLogAsync($"In Active A Daily Task With Id: {DailyTask.DTID} In The System", "DAILY TASK");


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
                    UpdatedDailyTask.RecurrenceDays = dailytask.RecurrenceDays;
                    UpdatedDailyTask.AgentId = dailytask.AgentId;
                    UpdatedDailyTask.LastUpdatedDate = dailytask.LastUpdatedDate;
                    alexsupportdb.DailyTasks.Update(UpdatedDailyTask);
                    await alexsupportdb.SaveChangesAsync();
                    await LogService.CreateSystemLogAsync($"Update A Daily Task With Id: {UpdatedDailyTask.DTID} In The System", "DAILY TASK");
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
        public async Task<bool> AssignDailyTask(DailyTasks dailytask)
        {
            var task = await alexsupportdb.DailyTasks.FirstOrDefaultAsync(u => u.DTID == dailytask.DTID);
            if (task != null)
            {
                Ticket ticket = new Ticket
                {
                    Subject = task.Subject,
                    Priority = task.Priority,
                    Issue = task.Issue,
                    CategoryID = task.CategoryID,
                    Due_Minutes = task.Due_Minutes,
                    OpenDate = DateTime.Now,
                    AgentID = dailytask.AgentId,
                    Status = "Assigned",
                    AssignDate = DateTime.Now,
                    UID = dailytask.UID,
                    LID = dailytask.LocationId ?? 0,
                };
                await alexsupportdb.Ticket.AddAsync(ticket);
                await alexsupportdb.SaveChangesAsync();
                await LogService.CreateSystemLogAsync($"Assign A Daily Task With Id: {dailytask.DTID} For {task.Agent.LoginName}", "DAILY TASK");


                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
