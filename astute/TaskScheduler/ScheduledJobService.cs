using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using astute.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace astute.TaskScheduler
{
    public class ScheduledJobService : BackgroundService
    {   
        private readonly IServiceProvider _serviceProvider;
        public ScheduledJobService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(1);
            var delay = nextRun - now;

            await Task.Delay(delay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var oracleService = scope.ServiceProvider.GetRequiredService<IOracleService>();
                    await oracleService.Get_Fortune_Discount();
                }

                nextRun = nextRun.AddDays(1);
                delay = nextRun - DateTime.UtcNow;
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}
