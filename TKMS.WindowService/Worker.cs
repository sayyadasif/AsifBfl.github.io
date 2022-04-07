using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Service.Interfaces;

namespace TKMS.WindowService
{
    public class Worker : BackgroundService
    {
        private static IConfiguration _configuration;
        private static IKitService _kitService;

        private readonly string cronSchedule;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        public Worker(
            IConfiguration config,
            IKitService kitService
            )
        {
            _configuration = config;
            _kitService = kitService;
            cronSchedule = _configuration.GetValue<string>("CRONTIME");
            _schedule = CrontabSchedule.Parse(cronSchedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                do
                {
                    var now = DateTime.Now;
                    var nextrun = _schedule.GetNextOccurrence(now);
                    if (now > _nextRun)
                    {
                        await ProcessReminders();
                        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                        Log.Information($"Next Schedular run at: {nextrun}");
                    }
                    await Task.Delay(1000 * 10, stoppingToken); //1 Min Interval
                }
                while (!stoppingToken.IsCancellationRequested);
            }
        }

        public async Task ProcessReminders()
        {
            Log.Information($"Schedular Start: {DateTime.Now}");

            Log.Information($"Assigned Start: {DateTime.Now}");

            try
            {
                var assignedResponse = await _kitService.UpdateKitAssigned();

                if (assignedResponse != null)
                {
                    if (assignedResponse.Success)
                    {
                        Log.Information($"Assigned success message: {assignedResponse.Message}");
                        Log.Information($"Assigned Process Completed: {DateTime.Now}");
                    }
                    else
                    {
                        Log.Information($"Assigned failed message: {assignedResponse.Message}");
                        Log.Information($"Assigned failed: {DateTime.Now}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information($"Assigned error message: {ex.Message}");
                Log.Information($"Assigned error failed: {DateTime.Now}");
            }

            Log.Information($"Assigned End: {DateTime.Now}");

            Log.Information($"Schedular End: {DateTime.Now}");
        }
    }
}
