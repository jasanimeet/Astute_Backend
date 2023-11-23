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
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var commonService = scope.ServiceProvider.GetRequiredService<ICommonService>();
                    await commonService.InsertErrorLog("Job started at: " + DateTime.UtcNow, "Job Schedule", "");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
