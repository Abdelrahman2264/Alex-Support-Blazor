using AlexSupport.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AlexSupport.Services.Extensions
{
    [Authorize]
    public class ScheduledBackgroundService : BackgroundService
    {
        private readonly ILogger<ScheduledBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5); // Run every 5 minutes

        public ScheduledBackgroundService(ILogger<ScheduledBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled Background Service is starting.");

            // Initial delay to avoid running immediately on startup
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Running scheduled task at {Time}...", DateTime.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dailyTaskRepository = scope.ServiceProvider.GetRequiredService<IDailyTaskRepository>();
                        await ProcessRecurringTasksAsync(dailyTaskRepository);
                    }

                    _logger.LogInformation($"Next run in {_interval.TotalMinutes} minutes...");
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (Exception ex) when (ex is not TaskCanceledException)
                {
                    _logger.LogError(ex, "Error in scheduled task");
                    // If error occurs, wait 1 minute before retrying
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }

            _logger.LogInformation("Scheduled Background Service is stopping.");
        }

        private async Task ProcessRecurringTasksAsync(IDailyTaskRepository dailyTaskRepository)
        {
            try
            {
                _logger.LogInformation("Fetching all daily tasks...");
                var allTasks = await dailyTaskRepository.GetAllDailyTasksAsync();
                _logger.LogInformation($"Found {allTasks.Count()} tasks to evaluate");

                var tasksToProcess = allTasks.Where(task =>
                    task.RecurrenceDays > 0 && // Ensure recurrence is set
                    (DateTime.Now - task.LastUpdatedDate).TotalDays >= task.RecurrenceDays
                ).ToList();

                _logger.LogInformation($"Found {tasksToProcess.Count} tasks meeting recurrence criteria");

                foreach (var task in tasksToProcess)
                {
                    try
                    {
                        _logger.LogInformation($"Processing task ID: {task.DTID}");

                        // Assign the task
                        await dailyTaskRepository.AssignDailyTask(task);

                        // Update the last updated date
                        task.LastUpdatedDate = DateTime.Now;
                        await dailyTaskRepository.UpdateDailyTaskAsync(task);

                        _logger.LogInformation($"Successfully assigned and updated task {task.DTID}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error processing task {task.DTID}");
                        // Continue with next task even if this one fails
                    }
                }

                _logger.LogInformation($"Completed processing {tasksToProcess.Count} recurring tasks");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during recurring task processing");
                throw;
            }
        }
    }
}