using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Service.Ioc;

namespace TKMS.WindowService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            // Set up to run as a service if not in Debug mode or if a command line argument is not --console
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService)
            {
                var processModule = Process.GetCurrentProcess().MainModule;
                if (processModule != null)
                {
                    var pathToExe = processModule.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                    Directory.SetCurrentDirectory(pathToContentRoot);
                }
            }
            Log.Logger = new LoggerConfiguration()
                              .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                              .CreateLogger();
            Log.Information($"Service started at: {DateTime.Now}");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IConfiguration>(hostContext.Configuration);
                    services.AddHostedService<Worker>();
                    services.RegisterServices(hostContext.Configuration);
                    services.Configure<IWorksSettings>(hostContext.Configuration.GetSection("IWorksConfig"));
                });
    }
}
