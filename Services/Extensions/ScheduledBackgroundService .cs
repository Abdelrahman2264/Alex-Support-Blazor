using AlexSupport.Repository.IRepository;
using Microsoft.Extensions.DependencyInjection;

namespace AlexSupport.Services.Extensions
{
    public class ScheduledBackgroundService : BackgroundService
    {
        private readonly ILogger<ScheduledBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ScheduledBackgroundService(ILogger<ScheduledBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 9, 30, 0);

                    // If it's already past 9:30 today, schedule for tomorrow
                    if (now > nextRunTime)
                    {
                        nextRunTime = nextRunTime.AddDays(1);
                    }

                    var delayTime = nextRunTime - now;
                    _logger.LogInformation($"Next run scheduled for {nextRunTime} ({delayTime.TotalHours:F2} hours from now)");

                    // Wait until the next scheduled time (or until canceled)
                    await Task.Delay(delayTime, stoppingToken);

                    // If we were canceled during the wait, break out
                    if (stoppingToken.IsCancellationRequested) break;

                    _logger.LogInformation("Running scheduled task at {Time}...", DateTime.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dailyTask = scope.ServiceProvider.GetRequiredService<IDailyTaskRepository>();
                        await DoWorkAsync(dailyTask);
                    }
                }
                catch (Exception ex) when (ex is not TaskCanceledException)
                {
                    _logger.LogError(ex, "Error in scheduled task");
                    // If error occurs, wait 5 minutes before retrying
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }

            _logger.LogInformation("Scheduled Background Service is stopping.");
        }

        private async Task DoWorkAsync(IDailyTaskRepository dailyTask)
        {
            try
            {
                _logger.LogInformation("Starting database operations...");

                var items = await dailyTask.GetAllDailyTasksAsync();
                _logger.LogInformation($"Found {items.Count()} tasks to process");

                foreach (var item in items)
                {
                    // DateTime is non-nullable, so no need for HasValue check
                    // Calculate days since last update
                    var daysSinceUpdate = (DateTime.UtcNow - item.LastUpdatedDate).TotalDays;

                    // Assuming TypeName is a numeric value representing days threshold
                    if (daysSinceUpdate >= item.TypeName)
                    {
                        await dailyTask.AssignDailyTask(item);

                        // Update timestamp after successful assignment
                        item.LastUpdatedDate = DateTime.UtcNow;
                        await dailyTask.UpdateDailyTaskAsync(item);

                        _logger.LogInformation($"Assigned and updated task {item.DTID}");
                    }
                }

                _logger.LogInformation("Database operations completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during database operations");
                throw;
            }
        }
    }
}